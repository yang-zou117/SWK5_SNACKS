using Dal.Domain;

namespace Dal.Interface;

public interface IDeliveryConditionDao
{
    Task<int> InsertDeliveryConditionAsync(DeliveryCondition newCondition);
    Task<IEnumerable<DeliveryCondition>> GetDeliveryConditionsForRestaurantAsync(int restaurantId);
    Task<bool> UpdateDeliveryConditionAsync(DeliveryCondition condition);
    Task<bool> DeleteDeliveryConditionAsync(int deliveryConditionId);
    Task<DeliveryCondition ?> GetDeliveryConditionByIdAsync(int deliveryConditionId);
}
