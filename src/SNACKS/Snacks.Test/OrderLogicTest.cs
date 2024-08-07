

using Dal.Domain;
using Snacks.Logic;

namespace Snacks.Test;
public class OrderLogicTest
{
    private readonly IOrderLogic orderLogic;

    public OrderLogicTest()
    {
        orderLogic = new OrderLogic();
    }

    
    [Fact]
    public async Task GetOrderValueAsync_Should_Return_Right_Value()
    {
        int restaurantId = 1;
        OrderItem item1 = new OrderItem(0, 0, 1, 2); // item id 1 and amount 2
        OrderItem item2 = new OrderItem(0, 0, 2, 3); // item id 2 and amount 3
        OrderItem item3 = new OrderItem(0, 0, 3, 1); // item id 3 and amount 1

        OrderItem[] items = new OrderItem[] { item1, item2, item3 };

        // value for the price --> db insert script
        decimal expected = 2 * 9.90m + 3 * 10.90m + 1 * 8.90m;
        decimal? result = await orderLogic.GetOrderValueAsync(restaurantId, items);

        Assert.NotNull(result);
        Assert.Equal(expected, result);

    }

    [Fact]
    public async Task GetOrderValueAsync_Should_Return_Null()
    {
        int restaurantId = 1;
        OrderItem item1 = new OrderItem(0, 0, 10, 2); // item id 1 and amount 2
        OrderItem item2 = new OrderItem(0, 0, 2, 3); // item id 2 and amount 3
        OrderItem item3 = new OrderItem(0, 0, 3, 1); // item id 3 and amount 1

        OrderItem[] items = new OrderItem[] { item1, item2, item3 };

        decimal? result = await orderLogic.GetOrderValueAsync(restaurantId, items);

        Assert.Null(result); // items belong not to same restaurant

    }

    [Fact]
    public async Task GetDistanceToDeliveryAddressAsync_Should_Return_Right_Value()
    {
        int restaurantId = 1;
        Address deliveryAddress = 
            new Address(0, 4540, "TestCity", "TestStreet", 14, 16.1234m, 48.1234m, "Test Free Text");

        int distance = 
            await orderLogic.GetDistanceToDeliveryAddressAsync(restaurantId, deliveryAddress);
        int expected = 20; 
        Assert.Equal(distance, expected);
    }

    [Fact]
    public async Task GetDeliveryCostsAsync_Should_Return_Right_Value()
    {
        int restaurantId = 1;
        int distance = 20;
        decimal orderValue = 10.0m;

        decimal? result = await orderLogic.GetDeliveryCostsAsync(restaurantId, distance, orderValue);
        decimal expected = 8.0m;
        Assert.Equal(result, expected);
    }

    [Fact]
    public async Task GetDeliveryCostsAsync_Should_Return_Null()
    {
        int restaurantId = 1;
        int distance = 250;
        decimal orderValue = 100.0m;

        // no suitable delivery condition as distance is too high
        decimal? result = await orderLogic.GetDeliveryCostsAsync(restaurantId, distance, orderValue);
        Assert.Null(result);
    }

}

 
