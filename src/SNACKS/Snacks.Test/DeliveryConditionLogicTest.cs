


using Dal.Domain;
using Snacks.Logic;

namespace Snacks.Test;
public class DeliveryConditionLogicTest
{
    private readonly IDeliveryConditionLogic logic; 

    public DeliveryConditionLogicTest()
    {
        logic = new DeliveryConditionLogic();
    }

    [Fact]
    public async Task IsDeliveryConditionValidAsync_Should_Return_true()
    {
        DeliveryCondition newCondition = new DeliveryCondition(1, 3, 0, 50, 10, 49.99m, 2, 10);
        Assert.True(await logic.IsDeliveryConditionValidAsync(newCondition));
    }

    [Fact]
    public async Task IsDeliveryConditionValidAsync_Should_Return_false()
    {
        // already a condition for restaurant 3 for value between 50 and null
        // and distance between 0 and 50
        var newCondition =
            new DeliveryCondition(1, 3, 0, 50, 50, null, 2, 10);
        Assert.False(await logic.IsDeliveryConditionValidAsync(newCondition));
    }

}
