using Dal.Domain;
using static Dal.Domain.Order;

namespace Snacks.Logic;

public interface IOrderLogic
{
    Task<IEnumerable<RestaurantDistanceResult>> GetRestaurantsInProximityAsync(double latitude, double longitude, int maxDistance, bool shouldBeOpen);
    Task<decimal?> GetOrderValueAsync(int restaurantId, OrderItem[] orderedItems);
    Task<int> GetDistanceToDeliveryAddressAsync(int restaurantId, Address deliveryAddress);
    Task<decimal?> GetDeliveryCostsAsync(int restaurantId, int distance, decimal orderValue);
    Task<int> AddAddressAsync(Address address);
    Task<int> AddOrderAsync(Order order);
    Task<bool> AddOrderItemAsync(OrderItem orderItem);
    Task<Order?> GetOrderAsync(int orderId);
	Task<IEnumerable<Order>> GetOrdersForRestaurantIdAsync(int restaurantId);
	Task<bool> UpdateOrderAsync(Order order);
    Task<ChangeOrderStatusLink?> GetChangeOrderStatusLinkAsync(int orderId, OrderStatus newStatus);
    Task<bool> AddChangeOrderStatusLinkAsync(ChangeOrderStatusLink link);
}
