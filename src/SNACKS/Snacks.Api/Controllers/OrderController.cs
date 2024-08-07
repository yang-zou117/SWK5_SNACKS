using AutoMapper;
using Dal.Domain;
using Microsoft.AspNetCore.Mvc;
using Snacks.Api.Dtos;
using Snacks.Api.HostedServices;
using Snacks.Logic;
using System.Transactions;
using static Dal.Domain.Order;

namespace Snacks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderLogic logic;
    private readonly IRestaurantLogic restaurantLogic;
    private readonly IApiKeyLogic apiKeyLogic;
    private readonly IMapper mapper;
    private readonly SendOrderToWebhookUrlService service; 

    public OrderController(IOrderLogic logic, IMapper mapper, IApiKeyLogic apiKeyLogic,
                           IRestaurantLogic restaurantLogic, SendOrderToWebhookUrlService service)
    {
        this.logic = logic ?? throw new ArgumentNullException(nameof(logic));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.apiKeyLogic = apiKeyLogic ?? throw new ArgumentNullException(nameof(apiKeyLogic));
        this.service = service ?? throw new ArgumentNullException(nameof(service));
        this.restaurantLogic = restaurantLogic ?? throw new ArgumentNullException(nameof(restaurantLogic));
    }

    // Use Case 5: find Restaurant in proximity 
    [HttpGet("findRestaurants")]
    public async Task<IEnumerable<RestaurantDistanceResultDto>> 
        GetRestaurantInProximity(int maxDistance, double latitude, double longitude, bool shouldBeOpen)
    {
        var restaurants = 
            await logic.GetRestaurantsInProximityAsync(latitude, longitude, maxDistance, shouldBeOpen);
        return mapper.Map<IEnumerable<RestaurantDistanceResultDto>>(restaurants);
    }

    // Use Case 7: calculate the price for a selection of menu items
    [HttpPost("calculatePrice")]
    public async Task<ActionResult<decimal>> CalculatePrice(PriceCalculationDto priceCalculationDto)
    {
        // calculate first the order value
        var restaurantId = priceCalculationDto.RestaurantId;
        var orderedItems = mapper.Map<OrderItem[]>(priceCalculationDto.OrderedItems);
        var orderValue = await logic.GetOrderValueAsync(restaurantId, orderedItems);
        if (orderValue is null)
            return BadRequest(StatusInfo.InvalidOrderedItemsSelection(restaurantId));

        // calculate the distance
        var deliveryAddress = mapper.Map<Address>(priceCalculationDto.DeliveryAddress);
        var distance = await logic.GetDistanceToDeliveryAddressAsync(restaurantId, deliveryAddress);

        // get delivery costs
        var deliveryCosts = await logic.GetDeliveryCostsAsync(restaurantId, distance, orderValue.Value);
        if (deliveryCosts is null)
            return BadRequest(StatusInfo.NoSuitableDeliveryCondition(restaurantId, distance, orderValue.Value));
        
        return orderValue.Value + deliveryCosts.Value;
    }
    
    // Use Case 8: customer orders a selction of menu items
    [HttpPost("placeOrder")]
    public async Task<ActionResult<Order>> PostOrder(PlaceOrderDto dto) 
    {
        // check price first 
        PriceCalculationDto priceCalculationDto = new()
        {
            RestaurantId = dto.Order.RestaurantId,
            DeliveryAddress = dto.DeliveryAddress,
            OrderedItems = dto.OrderedItems
        };
        var orderCosts = await CalculatePrice(priceCalculationDto);
        if (orderCosts.Result is BadRequestObjectResult)
            return BadRequest(StatusInfo.InvalidOrderItems());

		// add address to database
		var address = mapper.Map<Address>(dto.DeliveryAddress);
		var addressId = await logic.AddAddressAsync(address);
		address.AddressId = addressId;

		// add order to database
		var order = mapper.Map<Order>(dto.Order);
		order.AddressId = addressId;
		order.OrderCosts = orderCosts.Value;
		order.OrderedDate = DateTime.Now;
		var orderId = await logic.AddOrderAsync(order);
		order.OrderId = orderId;

		// insert change_order_status_link for each state
		foreach (OrderStatus status in Enum.GetValues(typeof(OrderStatus)))
		{
			var linkString = await GenerateChangeOrderStatusLink(orderId, status);
			var link = new ChangeOrderStatusLink(linkString, orderId, status);
			await logic.AddChangeOrderStatusLinkAsync(link);
		}

		// add order items to database
		var orderItems = mapper.Map<OrderItem[]>(dto.OrderedItems);
		foreach (var item in orderItems)
		{
			item.OrderId = orderId;
			await logic.AddOrderItemAsync(item);
		}

		// call webapi of restaurant in a background service
		var orderForExternalApi = new OrderForExternalApiDto
		{
			Order = order,
			DeliveryAddress = dto.DeliveryAddress,
			OrderedItems = dto.OrderedItems
		};
		_ = service.SendOrderToWebhookUrlAsync(orderForExternalApi, restaurantLogic);

		return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, order);

    }
    
    [HttpGet("{orderId}")]
    public async Task<ActionResult<Order>> GetOrderById(int orderId)
    {
        var order = await logic.GetOrderAsync(orderId);
        if (order is null)
            return NotFound(StatusInfo.OrderNotFound(orderId));

        return order;
    }

	[HttpGet("restaurant/{restaurantId}")]
    public async Task<IEnumerable<Order>> GetOrdersForRestaurant(int restaurantId)
    {
		IEnumerable<Order> orders = 
            await logic.GetOrdersForRestaurantIdAsync(restaurantId);
        return orders; 
	}

	// Use case 12: get order status

	[HttpGet("status/{orderId}")]
    public async Task<ActionResult<OrderStatus>> GetOrderStatus(int orderId)
    {
        var order = await logic.GetOrderAsync(orderId);
        
        if (order is null)
            return NotFound(StatusInfo.OrderNotFound(orderId));

        return order.Status;
    }
    
    // Use case 10: change order status
    
    [HttpPut("status/{restaurantId}/{orderId}")]
    public async Task<ActionResult> UpdateOrderStatus(int orderId, int restaurantId, 
                                                      OrderStatusDto newStatus)
    {
        var order = await logic.GetOrderAsync(orderId);
        
        if (order is null)
            return NotFound(StatusInfo.OrderNotFound(orderId));
        if (order.RestaurantId != restaurantId)
            return BadRequest(StatusInfo.OrderNotBelongToRestaurant(orderId, restaurantId));

        order.Status = newStatus.Status;
        var updated = await logic.UpdateOrderAsync(order);

        return updated ? Ok(order) : StatusCode(500, StatusInfo.OrderNotUpdated(orderId));
    }


    // Use Case 11: link to change order status

    [HttpGet("status/link/{restaurantId}/{orderId}/{newStatus}")]
    public async Task<ActionResult<string>> GetChangeOrderStatusLink(int orderId, int restaurantId, 
                                                                     OrderStatus newStatus)
    {
        var order = await logic.GetOrderAsync(orderId);

        if (order is null)
            return NotFound(StatusInfo.OrderNotFound(orderId));
        if (order.RestaurantId != restaurantId)
            return BadRequest(StatusInfo.OrderNotBelongToRestaurant(orderId, restaurantId));

        var result = await logic.GetChangeOrderStatusLinkAsync(orderId, newStatus);
        if (result is null) // no link found -> generate new link
            return await GenerateChangeOrderStatusLink(orderId, newStatus);
        return result.Link;

    }

    [HttpGet("status/executeLink")]
    public async Task<ActionResult> ExecuteChangeOrderStatusLink(string token)
    {
        int orderId, restaurantId;
        OrderStatus newStatus;
        string apiKeyValue; 

        // decode token and get the information
        try
        {
            var tokenInformation = TokenService.DecodeToken(token);
            orderId = int.Parse(tokenInformation["OrderId"]);
            restaurantId = int.Parse(tokenInformation["RestaurantId"]);
            newStatus = Dal.Dao.Util.ParseOrderStatus(tokenInformation["NewStatus"]);
            apiKeyValue = tokenInformation["ApiKeyValue"];
        } 
        catch (Exception)
        {
            return BadRequest(StatusInfo.InvalidToken()); 
        }
        
        // check order
        var order = await logic.GetOrderAsync(orderId);
        if (order is null)
            return NotFound(StatusInfo.OrderNotFound(orderId));
        if (order.RestaurantId != restaurantId)
            return BadRequest(StatusInfo.OrderNotBelongToRestaurant(orderId, restaurantId));

        // check api key
        var tokenValid = await apiKeyLogic.ValidateApiKeyAsync(new ApiKey(restaurantId, apiKeyValue));
        if (!tokenValid)
            return BadRequest(StatusInfo.InvalidApiKeyToken(restaurantId, apiKeyValue));

        // update order
        order.Status = newStatus;
        var updated = await logic.UpdateOrderAsync(order);

        return updated ? Ok(order) : StatusCode(500, StatusInfo.OrderNotUpdated(orderId));
    }

    private async Task<string> GenerateChangeOrderStatusLink(int orderId, OrderStatus newStatus)
    {
        var order = await logic.GetOrderAsync(orderId);
        if (order is null)
            throw new ArgumentException($"Order with id {orderId} not found when generating link");

        var restaurantId = order.RestaurantId;
        var apiKey = await apiKeyLogic.GetApiKeyForRestaurantAsync(restaurantId);

        if (apiKey is null)
            throw new ArgumentException($"No api key found for restaurant with id {restaurantId}");

        var tokenInformation = new Dictionary<string, string>
        {
            {"OrderId", orderId.ToString() },
            {"RestaurantId", restaurantId.ToString() },
            {"NewStatus", newStatus.ToString() },
            {"ApiKeyValue", apiKey.ApiKeyValue }
        };
        var encodedToken = TokenService.EncodeToken(tokenInformation);

        var link = Url.ActionLink(nameof(ExecuteChangeOrderStatusLink), "Order",
                                  new { token = encodedToken });
        
        if (link is null)
            throw new ArgumentException($"Link could not be generated for order {orderId} and status {newStatus}");

        return link; 
    }

   
}
