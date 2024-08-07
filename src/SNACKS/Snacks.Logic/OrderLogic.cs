using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Logic;
public class OrderLogic: IOrderLogic
{
    private IMenuItemDao menuItemDao;
    private IRestaurantDao restaurantDao;
    private IAddressDao addressDao;
    private IOrderDao orderDao;
    private IDeliveryConditionDao deliveryConditionDao;
    private IChangeOrderStatusLinkDao changeOrderStatusLinkDao;
    private IOrderItemDao orderItemDao;

    public OrderLogic()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        menuItemDao = new MenuItemDao(connectionFactory);
        restaurantDao = new RestaurantDao(connectionFactory);
        addressDao = new AddressDao(connectionFactory);
        orderDao = new OrderDao(connectionFactory);
        deliveryConditionDao = new DeliveryConditionDao(connectionFactory);
        changeOrderStatusLinkDao = new ChangeOrderStatusLinkDao(connectionFactory);
        orderItemDao = new OrderItemDao(connectionFactory);
    }

    public async Task<IEnumerable<RestaurantDistanceResult>> GetRestaurantsInProximityAsync(double latitude, double longitude, 
                                                                                            int maxDistance, bool shouldBeOpen)
    {
        return await restaurantDao.GetRestaurantsInProximityAsync(latitude, longitude, maxDistance, shouldBeOpen);
    }

    
    // method calculates the value of the ordered items
    // returns null if menuItem selection is invalid
    public async Task<decimal?> GetOrderValueAsync(int restaurantId, OrderItem[] orderedItems)
    {
        decimal result = 0.0m;
        foreach(var ordered_item in orderedItems)
        {
            var menuItem = await menuItemDao.GetMenuItemByIdAsync(ordered_item.MenuItemId);
            if (menuItem is null || menuItem.RestaurantId != restaurantId)
                return null; // invalid menu item selection
            result += (menuItem.Price * ordered_item.Amount); 

        }

        return result; 
    }

    public async Task<int> GetDistanceToDeliveryAddressAsync(int restaurantId, Address deliveryAddress)
    {
        return await addressDao.GetDistanceToDeliveryAddressAsync(restaurantId, deliveryAddress);
    }

    // method calculates the delivery costs
    // returns null if delivery costs cannot be calculated
    public async Task<decimal?> GetDeliveryCostsAsync(int restaurantId, int distance, decimal orderValue)
    {
        var deliveryConditions = 
            await deliveryConditionDao.GetDeliveryConditionsForRestaurantAsync(restaurantId);

        // find the delivery condition that matches the distance and order value
        foreach (var dc in deliveryConditions)
        {
            if ( (distance >= dc.DistanceLowerThreshold && distance <= dc.DistanceUpperThreshold) &&
                 (orderValue >= dc.OrderValueLowerThreshold && 
                       (dc.OrderValueUpperThreshold is null || orderValue <= dc.OrderValueUpperThreshold)) 
               )
            {
                return dc.DeliveryCosts;
            }
        }
        return null; 

    }

    public async Task<int> AddAddressAsync(Address address)
    {
        return await addressDao.InsertAddressAsync(address);
    }

    public async Task<int> AddOrderAsync(Order order)
    {
        return await orderDao.InsertOrderAsync(order);
    }

    public async Task<Order?> GetOrderAsync(int orderId)
    {
        return await orderDao.GetOrderByIdAsync(orderId);
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        return await orderDao.UpdateOrderAsync(order);
    }

    public async Task<ChangeOrderStatusLink?> GetChangeOrderStatusLinkAsync(int orderId, Order.OrderStatus newStatus)
    {
        return await changeOrderStatusLinkDao.GetChangeOrderStatusLinkByNewStatusAsync(newStatus, orderId);
    }

    public async Task<bool> AddChangeOrderStatusLinkAsync(ChangeOrderStatusLink link)
    {
        return await changeOrderStatusLinkDao.InsertChangeOrderStatusLinkAsync(link);
    }

    public async Task<bool> AddOrderItemAsync(OrderItem orderItem)
    {
        return await orderItemDao.InsertOrderItemAsync(orderItem);
    }

	public async Task<IEnumerable<Order>> GetOrdersForRestaurantIdAsync(int restaurantId)
	{
		return await orderDao.GetOrdersForRestaurantIdAsync(restaurantId);
	}
}
