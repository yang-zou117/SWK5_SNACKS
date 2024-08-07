using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;
using static Dal.Domain.Order;

namespace Dal.Dao;

public class OrderDao : IOrderDao
{
    private readonly AdoTemplate template;

    public OrderDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    private Order MapRowToOrder(IDataRecord row)
    {
        return new Order(
            (int)row["order_id"],
            (int)row["restaurant_id"],
            (int)row["address_id"],
            (string)row["customer_name"],
            (string)row["phone_number"],
            (DateTime)row["ordered_date"],
            Util.ParseOrderStatus(((string)row["status"])),
            (decimal)row["order_costs"]
        );
    }

    public async Task<int> InsertOrderAsync(Order newOrder)
    {
        const string sql = "INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs) " +
                           "VALUES (@restaurant_id, @address_id, @customer_name, @phone_number, @status, @order_costs);" +
                           "SELECT LAST_INSERT_ID();";

        return await
            template.ExecuteScalarAsync<int>(sql,
            new QueryParameter("@restaurant_id", newOrder.RestaurantId),
            new QueryParameter("@address_id", newOrder.AddressId),
            new QueryParameter("@customer_name", newOrder.CustomerName),
            new QueryParameter("@phone_number", newOrder.PhoneNumber),
            new QueryParameter("@status", newOrder.Status.ToString()),
            new QueryParameter("@order_costs", newOrder.OrderCosts)
            );
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        const string sql = "SELECT * FROM `order` WHERE order_id = @order_id";

        return await template.QuerySingleAsync(sql, MapRowToOrder, new QueryParameter("@order_id", orderId));
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        return await template.ExecuteAsync("DELETE FROM `order` WHERE order_id = @order_id", new QueryParameter("@order_id", orderId)) == 1;
    }

    public async Task<bool> UpdateOrderAsync(Order order)
    {
        const string sql = "UPDATE `order` SET status = @status " +
                           "WHERE order_id = @order_id";

        return await template.ExecuteAsync(sql, new QueryParameter("@order_id", order.OrderId),
                                                new QueryParameter("@status", order.Status.ToString())) == 1;
    }

    public async Task<IEnumerable<Order>> GetOrdersForRestaurantIdAsync(int restaurantId)
    {
		const string sql = "SELECT * FROM `order` WHERE restaurant_id = @restaurant_id";
		return await template.QueryAsync(sql, MapRowToOrder, new QueryParameter("@restaurant_id", restaurantId));
	}

}
