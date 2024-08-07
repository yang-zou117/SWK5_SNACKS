using Dal.Domain;

namespace Dal.Interface;

public interface IOrderDao
{
    Task<int> InsertOrderAsync(Order newOrder);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<bool> DeleteOrderAsync(int orderId);
    Task<bool> UpdateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersForRestaurantIdAsync(int restaurantId);
}
