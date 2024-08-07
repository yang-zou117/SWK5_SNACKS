using Dal.Domain;

namespace Dal.Interface;

public interface IAddressDao
{
    Task<int> InsertAddressAsync(Address newAddress);
    Task<Address?> GetAddressByIdAsync(int addressId);
    Task<Address?> GetAddressForRestaurantAsync(int restaurantId);
    Task<bool> DeleteAddressAsync(int addressId);
    Task<int> GetDistanceToDeliveryAddressAsync(int restaurantId, Address deliveryAddress);
}
