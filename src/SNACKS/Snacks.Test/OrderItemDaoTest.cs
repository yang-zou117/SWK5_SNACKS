

using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;
using static Dal.Domain.Order;

namespace Snacks.Test;
public class OrderItemDaoTest
{
    private readonly IOrderDao orderDao;
    private readonly IOrderItemDao orderItemDao;

    public OrderItemDaoTest()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        orderDao = new OrderDao(connectionFactory);
        orderItemDao = new OrderItemDao(connectionFactory);
    }

    [Fact]
    public async Task Insert_Order_Item_Should_Insert()
    {
        var newOrder = new Order(0, 1, 1, "Test Name", "123456789",
                         DateTime.Now, OrderStatus.Pending, 10.00m);
        var newOrderId = await orderDao.InsertOrderAsync(newOrder);
        newOrder.OrderId = newOrderId;

        var newOrderItem = new OrderItem(0, newOrderId, 1, 1);
        var inserted = await orderItemDao.InsertOrderItemAsync(newOrderItem);
        Assert.True(inserted);
    }

    [Fact]
    public async Task Get_OrderItems_of_Order_Should_Return_Resulsts()
    {
        var orderItems = await orderItemDao.GetOrderItemsByOrderIdAsync(1); // order 1 already inserted by db script
        Assert.NotEmpty(orderItems);
        Assert.True(orderItems.Count() > 0);
        Assert.True(await orderItemDao.GetOrderItemByOrderItemIdAsync(1) != null); // order item 1 already inserted by db script
    }
    

}
