using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;
using static Dal.Domain.Order;

namespace Dal.Dao;

public class ChangeOrderStatusLinkDao : IChangeOrderStatusLinkDao
{
    private readonly AdoTemplate template;

    public ChangeOrderStatusLinkDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    public ChangeOrderStatusLink MapRowToChangeOrderStatusLink(IDataRecord row)
    {
        return new ChangeOrderStatusLink(
            (string)row["link"],
            (int)row["order_id"],
            Util.ParseOrderStatus(((string)row["new_status"]))
        );
    }

    public async Task<ChangeOrderStatusLink?> GetChangeOrderStatusLinkByNewStatusAsync(OrderStatus newStatus, 
                                                                                       int orderId)
    {
        const string sql = 
            "SELECT * FROM change_order_status_link WHERE new_status = @new_status AND order_id = @order_id";

        return await template.QuerySingleAsync(sql, MapRowToChangeOrderStatusLink,
                                                new QueryParameter("@new_status", newStatus.ToString()),
                                                new QueryParameter("@order_id", orderId));
    }

    public Task<IEnumerable<ChangeOrderStatusLink>> GetChangeOrderStatusLinksByOrderIdAsync(int orderId)
    {
        const string sql = "SELECT * FROM change_order_status_link WHERE order_id = @order_id";

        return template.QueryAsync(sql, MapRowToChangeOrderStatusLink,
                                   new QueryParameter("@order_id", orderId));
    }

    public async Task<bool> InsertChangeOrderStatusLinkAsync(ChangeOrderStatusLink link)
    {
        const string sql = "INSERT INTO change_order_status_link (link, order_id, new_status) " +
                           "VALUES (@link, @order_id, @new_status)";

        int rowsAffected = await template.ExecuteAsync(sql,
                                           new QueryParameter("@link", link.Link),
                                           new QueryParameter("@order_id", link.OrderId),
                                           new QueryParameter("@new_status", link.NewStatus.ToString()));
        return rowsAffected > 0;
    }
}
