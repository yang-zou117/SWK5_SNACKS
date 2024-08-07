using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Test;

public class DeliveryConditionDaoTest
{
    private readonly IDeliveryConditionDao deliveryConditionDao;

    public DeliveryConditionDaoTest()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        deliveryConditionDao = new DeliveryConditionDao(connectionFactory);
    }

    [Fact]
    public async Task InsertDeliveryCondition_Should_Work()
    {
        // delivery condition is that restaurant 1 does only deliver 
        // up to a distance of 20km and the minmum order value should be 30
        var newCondition = 
            new DeliveryCondition(1, 1, 31, 40, 30, null, 0, 30);

        var newConditionId = 
            await deliveryConditionDao.InsertDeliveryConditionAsync(newCondition);
        newCondition.DeliveryConditionId = newConditionId;

        Assert.True(newConditionId > 0);

        var deliveryConditions = await
            deliveryConditionDao.GetDeliveryConditionsForRestaurantAsync(1);

        Assert.Contains(newCondition, deliveryConditions.ToList());

    }

    [Fact]
    public async Task UpdateDeliveryCondition_Should_Work()
    {
        // condition for distance more than 10km and minimum value 20
        var newCondition =
            new DeliveryCondition(1, 1, 41, 50, 20, null, 0, 20);

        var newConditionId =
            await deliveryConditionDao.InsertDeliveryConditionAsync(newCondition);
        newCondition.DeliveryConditionId = newConditionId;
        newCondition.DeliveryCosts = 5; // increase delivery costs

        // check if condition has been updated
        Assert.True(await deliveryConditionDao.UpdateDeliveryConditionAsync(newCondition));   
    }

    [Fact]
    public async Task DeleteDeliveryCondition_Should_Work()
    {
        // condition for distance up to 5km and minimum value 10
        var newCondition =
            new DeliveryCondition(1, 1, 51, 60, 10, null, 0.10m, 10);

        var newConditionId =
            await deliveryConditionDao.InsertDeliveryConditionAsync(newCondition);
        newCondition.DeliveryConditionId = newConditionId;

        // check if condition has been deleted
        Assert.True(await deliveryConditionDao.DeleteDeliveryConditionAsync(newConditionId));
    }
}
