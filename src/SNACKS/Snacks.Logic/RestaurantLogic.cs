using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Logic;
public class RestaurantLogic : IRestaurantLogic
{
    private IAddressDao addressDao;
    private IRestaurantDao restaurantDao;
    private IOpeningHoursDao openingHoursDao;
    private IClosingDayDao closingDayDao;

    public RestaurantLogic()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        addressDao = new AddressDao(connectionFactory);
        restaurantDao = new RestaurantDao(connectionFactory);
        openingHoursDao = new OpeningHoursDao(connectionFactory);
        closingDayDao = new ClosingDayDao(connectionFactory);

    }

    public async Task<int> AddAddressAsync(Address address)
    {
        return await addressDao.InsertAddressAsync(address);
    }

    public async Task<int> AddRestaurantAsync(Restaurant restaurant)
    {
        return await restaurantDao.InsertRestaurantAsync(restaurant);
    }

    public async Task<int> AddOpeningHoursAsync(OpeningHours openingHours)
    {
        return await openingHoursDao.InsertOpeningHoursAsync(openingHours);
    }

    public async Task<int> AddClosingDayAsync(ClosingDay closingDay)
    {
        return await closingDayDao.InsertClosingDayAsync(closingDay);
    }

    public async Task<bool> DeleteRestaurantAsync(int restaurantId)
    {
        return await restaurantDao.DeleteRestaurantAsync(restaurantId);
    }

    public async Task<Address?> GetAddressByIdAsync(int addressId)
    {
        return await addressDao.GetAddressByIdAsync(addressId);
    }

    public async Task<Address?> GetAddressForRestaurantAsync(int restaurantId)
    {
        return await addressDao.GetAddressForRestaurantAsync(restaurantId);
    }

    public async Task<IEnumerable<OpeningHours>> GetOpeningHoursForRestaurantAsync(int restaurantId)
    {
        return await openingHoursDao.GetOpeningHoursForRestaurantAsync(restaurantId);
    }

    public async Task<IEnumerable<ClosingDay>> GetClosingDaysForRestaurantAsync(int restaurantId)
    {
        return await closingDayDao.GetClosingDaysForRestaurantAsync(restaurantId);
    }

    public async Task<Restaurant?> GetRestaurantAsync(int restaurantId)
    {
        return await restaurantDao.GetRestaurantByIdAsync(restaurantId);
    }

    public async Task<bool> DeleteOpeningHoursAsync(int openingHoursId)
    {
        return await openingHoursDao.DeleteOpeningHoursAsync(openingHoursId);
    }

    public async Task<bool> DeleteClosingDayAsync(string weekDay, int restaurantId)
    {
        return await closingDayDao.DeleteClosingDayAsync(weekDay, restaurantId);
    }

}
