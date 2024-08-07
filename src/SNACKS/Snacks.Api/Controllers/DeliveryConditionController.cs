using AutoMapper;
using Dal.Domain;
using Microsoft.AspNetCore.Mvc;
using Snacks.Api.Dtos;
using Snacks.Logic;

namespace Snacks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryConditionController : Controller
{
    private readonly IDeliveryConditionLogic logic;
    private readonly IMapper mapper;

    public DeliveryConditionController(IDeliveryConditionLogic logic, IMapper mapper)
    {
        this.logic = logic ?? throw new ArgumentNullException(nameof(logic));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("{deliveryConditionId}")]
    public async Task<ActionResult<DeliveryConditionDto>> GetDeliveryConditionById(int deliveryConditionId)
    {
        var deliveryCondition = await logic.GetDeliveryConditionByIdAsync(deliveryConditionId);
        if (deliveryCondition == null)
        {
            return NotFound(StatusInfo.InvalidDeliveryConditionId(deliveryConditionId));
        }
        return mapper.Map<DeliveryConditionDto>(deliveryCondition);
    }

    [HttpGet("restaurant/{restaurantId}")]
    public async Task<IEnumerable<DeliveryConditionDto>> GetDeliveryConditionsByRestaurantId(int restaurantId)
    {
        IEnumerable<DeliveryCondition> deliveryConditions = 
            await logic.GetDeliveryConditionsByRestaurantIdAsync(restaurantId);
        return mapper.Map<IEnumerable<DeliveryConditionDto>>(deliveryConditions);
    }

    [HttpPost("{restaurantId}")]
    public async Task<ActionResult<DeliveryConditionDto>> AddDeliveryCondition(int restaurantId,
                                                                               [FromBody] DeliveryConditionForCreationDto dto)
    {
        // check if delivery condition is valid
        DeliveryCondition deliveryCondition = mapper.Map<DeliveryCondition>(dto);
        deliveryCondition.RestaurantId = restaurantId;
        var isValid = await logic.IsDeliveryConditionValidAsync(deliveryCondition);
        if (!isValid)
            return BadRequest(StatusInfo.InvalidDeliveryCondition());

        // add delivery condition
        deliveryCondition.DeliveryConditionId = 
            await logic.AddDeliveryConditionAsync(deliveryCondition);
        return CreatedAtAction(nameof(GetDeliveryConditionById),
                               routeValues: new { deliveryConditionId = 
                                                  deliveryCondition.DeliveryConditionId },
                               mapper.Map<DeliveryConditionDto>(deliveryCondition));
    }

    [HttpPut("{restaurantId}/{deliveryConditionId}")]
    public async Task<ActionResult> UpdateDeliveryCondition(int restaurantId, int deliveryConditionId,
                                                            DeliveryConditionForUpdateDto dto)
    {
        // check if delivery condition exists
        var deliveryCondition = await logic.GetDeliveryConditionByIdAsync(deliveryConditionId);
        if (deliveryCondition == null)
            return NotFound(StatusInfo.InvalidDeliveryConditionId(deliveryConditionId));
        if (deliveryCondition.RestaurantId != restaurantId)
            return BadRequest(StatusInfo.InvalidDeliveryConditionId(deliveryConditionId));
        mapper.Map(dto, deliveryCondition);

        // check if the delivery condition is valid
        var isValid = await logic.IsDeliveryConditionValidAsync(deliveryCondition);
        if (!isValid)
            return BadRequest(StatusInfo.InvalidDeliveryCondition());

        // update the delivery condition
        var updated = await logic.UpdateDeliveryConditionAsync(deliveryCondition);
        return updated ? NoContent() : StatusCode(500);

    }

    [HttpDelete("{restaurantId}/{deliveryConditionId}")]
    public async Task<ActionResult> DeleteDeliveryCondition(int restaurantId, int deliveryConditionId)
    {
        // get delivery condition
        var deliveryCondition = await logic.GetDeliveryConditionByIdAsync(deliveryConditionId);
        if (deliveryCondition == null)
            return NotFound(StatusInfo.InvalidDeliveryConditionId(deliveryConditionId));
        if (deliveryCondition.RestaurantId != restaurantId)
            return BadRequest(StatusInfo.InvalidDeliveryConditionId(deliveryConditionId));

        var deleted = await logic.DeleteDeliveryConditionAsync(deliveryConditionId);
        return deleted ? NoContent() : StatusCode(500);
    }
}
