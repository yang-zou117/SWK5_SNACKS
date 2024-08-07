

using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;
using static Dal.Domain.Order;

namespace Snacks.Test;
public class OrderDaoTest
{
    private readonly IOrderDao orderDao;

    public OrderDaoTest()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        orderDao = new OrderDao(connectionFactory);
    }

    [Fact]
    public async Task InsertOrder_ShouldInsert()
    {

        var newOrder = new Order(0, 1, 1, "Test Name", "123456789", 
                                 DateTime.Now, OrderStatus.Pending, 10.00m);
        var newOrderId = await orderDao.InsertOrderAsync(newOrder);
        newOrder.OrderId = newOrderId;
        Assert.True(newOrderId > 0);
        Assert.Equal(newOrder, await orderDao.GetOrderByIdAsync(newOrderId));
        
    }

    [Fact] 
    public async Task UpdateOrder_Should_Update()
    {
        var newOrder = new Order(0, 1, 1, "Test Name", "123456789",
                         DateTime.Now, OrderStatus.Pending, 10.00m);
        var newOrderId = await orderDao.InsertOrderAsync(newOrder);
        newOrder.OrderId = newOrderId;
        newOrder.Status = OrderStatus.BeingPrepared;

        Assert.True(await orderDao.UpdateOrderAsync(newOrder));
        Assert.Equal(newOrder, await orderDao.GetOrderByIdAsync(newOrderId));
    }
    

}
