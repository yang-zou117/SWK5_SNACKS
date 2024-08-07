using Dal.Domain;

namespace Dal.Interface;

public interface IOrderItemDao
{
    Task<bool> InsertOrderItemAsync(OrderItem newOrderItem);
    Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
    Task<OrderItem?> GetOrderItemByOrderItemIdAsync(int orderItemId);
}
