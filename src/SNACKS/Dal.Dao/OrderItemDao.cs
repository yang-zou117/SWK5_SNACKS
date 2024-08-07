using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;
public class OrderItemDao : IOrderItemDao
{
    private readonly AdoTemplate template;

    public OrderItemDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    private OrderItem MapRowToOrderItem(IDataRecord row)
    {
        return new OrderItem(
            (int)row["order_item_id"],
            (int)row["order_id"],
            (int)row["menu_item_id"],
            (int)row["amount"]
        );
    }

    public async Task<OrderItem?> GetOrderItemByOrderItemIdAsync(int orderItemId)
    {
        const string sql = "SELECT * FROM order_item WHERE order_item_id = @OrderItemId";

        return await template.QuerySingleAsync(sql, MapRowToOrderItem,
                                               new QueryParameter("@OrderItemId", orderItemId));
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        const string sql = "SELECT * FROM order_item WHERE order_id = @OrderId";

        return await template.QueryAsync(sql, MapRowToOrderItem,
                                         new QueryParameter("@OrderId", orderId));
    }

    public async Task<bool> InsertOrderItemAsync(OrderItem newOrderItem)
    {
        const string sql = "INSERT INTO order_item (order_id, menu_item_id, amount) " +
                           "VALUES (@order_id, @menu_item_id, @amount);";

        int rowsAffected = await template.ExecuteAsync(sql,
            new QueryParameter("@order_id", newOrderItem.OrderId),
            new QueryParameter("@menu_item_id", newOrderItem.MenuItemId),
            new QueryParameter("@amount", newOrderItem.Amount));

        return rowsAffected > 0;
    }
}
