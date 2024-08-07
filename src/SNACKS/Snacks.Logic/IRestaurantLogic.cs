using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Logic;

public interface IRestaurantLogic
{

    Task<int> AddAddressAsync(Address address);
    Task<int> AddRestaurantAsync(Restaurant restaurant);
    Task<int> AddOpeningHoursAsync(OpeningHours openingHours);
    Task<int> AddClosingDayAsync(ClosingDay closingDay);
    Task<bool> DeleteRestaurantAsync(int restaurantId);
    Task<Address?> GetAddressByIdAsync(int addressId);
    Task<Address?> GetAddressForRestaurantAsync(int restaurantId);
    Task<IEnumerable<OpeningHours>> GetOpeningHoursForRestaurantAsync(int restaurantId);
    Task<IEnumerable<ClosingDay>> GetClosingDaysForRestaurantAsync(int restaurantId);
    Task<Restaurant?> GetRestaurantAsync(int restaurantId);
    Task<bool> DeleteOpeningHoursAsync(int openingHoursId);
    Task<bool> DeleteClosingDayAsync(string weekDay, int restaurantId);

}
