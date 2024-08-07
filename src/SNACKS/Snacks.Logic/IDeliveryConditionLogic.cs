using Dal.Domain;

namespace Snacks.Logic;
public interface IDeliveryConditionLogic
{
    Task<DeliveryCondition?> GetDeliveryConditionByIdAsync(int deliveryConditionId);
    Task<IEnumerable<DeliveryCondition>> GetDeliveryConditionsByRestaurantIdAsync(int restaurantId);
    Task<int> AddDeliveryConditionAsync(DeliveryCondition deliveryCondition);
    Task<bool> UpdateDeliveryConditionAsync(DeliveryCondition deliveryCondition);
    Task<bool> DeleteDeliveryConditionAsync(int deliveryConditionId);
    Task<bool> IsDeliveryConditionValidAsync(DeliveryCondition deliveryCondition);
}
