using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Logic;
public class DeliveryConditionLogic : IDeliveryConditionLogic
{

    private IDeliveryConditionDao deliveryConditionDao;
    private IRestaurantDao restaurantDao; 

    public DeliveryConditionLogic()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        deliveryConditionDao = new DeliveryConditionDao(connectionFactory);
        restaurantDao = new RestaurantDao(connectionFactory);
    }

    public async Task<int> AddDeliveryConditionAsync(DeliveryCondition deliveryCondition)
    {
        return await 
            deliveryConditionDao.InsertDeliveryConditionAsync(deliveryCondition);
    }

    public async Task<bool> DeleteDeliveryConditionAsync(int deliveryConditionId)
    {
        return await
            deliveryConditionDao.DeleteDeliveryConditionAsync(deliveryConditionId);
    }

    public async Task<DeliveryCondition?> GetDeliveryConditionByIdAsync(int deliveryConditionId)
    {
        return await deliveryConditionDao.GetDeliveryConditionByIdAsync(deliveryConditionId);
    }

    public async Task<IEnumerable<DeliveryCondition>> GetDeliveryConditionsByRestaurantIdAsync(int restaurantId)
    {
        return await deliveryConditionDao.GetDeliveryConditionsForRestaurantAsync(restaurantId);
    }

    public async Task<bool> IsDeliveryConditionValidAsync(DeliveryCondition dc)
    {

        // check if restaurant exists
        var restaurant = await restaurantDao.GetRestaurantByIdAsync(dc.RestaurantId);
        if (restaurant is null)
            return false; 

        // check if there is another delivery condition with the same range
        var deliveryConditions = await 
            deliveryConditionDao.GetDeliveryConditionsForRestaurantAsync(dc.RestaurantId);
        foreach (var curr_dc in deliveryConditions)
        {
            if (curr_dc.DeliveryConditionId == dc.DeliveryConditionId)
                continue;

            if (IsConflictingRanges(dc, curr_dc))
                return false; 
        }

        return true; 
    }

    public async Task<bool> UpdateDeliveryConditionAsync(DeliveryCondition deliveryCondition)
    {
        return await
            deliveryConditionDao.UpdateDeliveryConditionAsync(deliveryCondition);
    }

    private bool IsConflictingRanges(DeliveryCondition dc1, DeliveryCondition dc2)
    {
        // assumption that lowerThreshold is always lower than UpperThreashold

        // distance range of dc1 greater or less than that of dc2
        if (dc1.DistanceUpperThreshold < dc2.DistanceLowerThreshold ||
            dc1.DistanceLowerThreshold > dc2.DistanceUpperThreshold)
            return false;

        // order value range of dc1 less or greater than that of dc2
        if ((dc1.OrderValueUpperThreshold is not null &&
             dc1.OrderValueUpperThreshold < dc2.OrderValueLowerThreshold) ||
            (dc2.OrderValueUpperThreshold is not null &&
             dc1.OrderValueLowerThreshold > dc2.OrderValueUpperThreshold))
            return false;

        // order_value_upper value of dc1 is infinite and range is greater than dc2
        if (dc1.OrderValueUpperThreshold is null &&
            dc2.OrderValueUpperThreshold is not null &&
            dc1.OrderValueLowerThreshold > dc2.OrderValueUpperThreshold)
            return false; 

        return true; 
    }

    
    
}
