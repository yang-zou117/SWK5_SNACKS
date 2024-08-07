using Dal.Domain;
using static Dal.Domain.Order;

namespace Dal.Interface;
public interface IChangeOrderStatusLinkDao
{
    Task<bool> InsertChangeOrderStatusLinkAsync(ChangeOrderStatusLink link);
    Task<ChangeOrderStatusLink?> GetChangeOrderStatusLinkByNewStatusAsync(OrderStatus newStatus, int orderId);
    Task<IEnumerable<ChangeOrderStatusLink>> GetChangeOrderStatusLinksByOrderIdAsync(int orderId);
}
