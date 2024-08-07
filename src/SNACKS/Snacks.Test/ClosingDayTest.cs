using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Test;

public class ClosingDayTest
{
    private readonly IClosingDayDao closingDayDao;
    private readonly IRestaurantDao restaurantDao;

    public ClosingDayTest()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        closingDayDao = new ClosingDayDao(connectionFactory);
        restaurantDao = new RestaurantDao(connectionFactory);
    }

    [Fact]
    public async Task InsertClosingDay_ShouldInsert()
    {
        var newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        var newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);
        
        var newClosingDay = new ClosingDay("Monday", newRestaurantId);
        await closingDayDao.InsertClosingDayAsync(newClosingDay);

        var closingDays = await closingDayDao.GetClosingDaysForRestaurantAsync(newRestaurantId);
        Assert.NotNull(closingDays); 
        Assert.True(closingDays.Count() > 0);
        
    }

    [Fact]
    public async Task GetClosingDaysForRestaurant_ShouldReturnClosingDays()
    {
        var newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        var newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);

        var newClosingDay = new ClosingDay("Monday", newRestaurantId);
        await closingDayDao.InsertClosingDayAsync(newClosingDay);

        var closingDays = await closingDayDao.GetClosingDaysForRestaurantAsync(newRestaurantId);
        var enumerable = closingDays as ClosingDay[] ?? closingDays.ToArray();
        Assert.True(enumerable.Any());
        Assert.Contains(newClosingDay, enumerable.ToList());
    }

    [Fact]
    public async Task DeleteClosingDay_ShouldDeleteClosingDay()
    {
        Restaurant newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        int newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);

        ClosingDay newClosingDay = new ClosingDay("Monday", newRestaurantId);
        await closingDayDao.InsertClosingDayAsync(newClosingDay);

        Assert.True(await closingDayDao.DeleteClosingDayAsync("Monday", newRestaurantId));
    }

}
