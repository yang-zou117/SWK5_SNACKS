using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Test;

public class RestaurantDaoTests
{
    private readonly IRestaurantDao restaurantDao;

    public RestaurantDaoTests()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        restaurantDao = new RestaurantDao(connectionFactory);
    }

    [Fact]
    public async Task InsertRestaurantAsync_ShouldInsertRestaurantAndReturnId()
    {
        Restaurant newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        var newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);
        newRestaurant.RestaurantId = newRestaurantId;

        Assert.True(newRestaurantId > 0);
        Assert.Equal(newRestaurant, await restaurantDao.GetRestaurantByIdAsync(newRestaurantId));
    }

    [Fact]
    public async Task GetRestaurantByIdAsync_ShouldGetRestaurant()
    {
        var newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        var newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);
        newRestaurant.RestaurantId = newRestaurantId;

        Assert.Equal(newRestaurant, await restaurantDao.GetRestaurantByIdAsync(newRestaurantId));
    }

    [Fact]
    public async Task DeleteRestaurantAsync_ShouldDeleteRestaurant()
    {
        var newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        var newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);
        newRestaurant.RestaurantId = newRestaurantId;

        Assert.True(await restaurantDao.DeleteRestaurantAsync(newRestaurantId));
        Assert.Null(await restaurantDao.GetRestaurantByIdAsync(newRestaurantId));
    }

    [Fact]
    public async Task UpdateRestaurantAsync_ShouldUpdateRestaurant()
    {
        var newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        var newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);
        newRestaurant.RestaurantId = newRestaurantId;

        newRestaurant.WebhookUrl = "http://testUpdated.com";
        newRestaurant.RestaurantName = "Updated Restaurant";

        Assert.True(await restaurantDao.UpdateRestaurantAsync(newRestaurant));
        Assert.Equal(newRestaurant, await restaurantDao.GetRestaurantByIdAsync(newRestaurantId));

    }

    [Fact]
    public void InsertRestaurantWithInvalidAddressId_Throws()
    {
        Restaurant newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 9999);
        Assert.ThrowsAsync<Exception>(async () => await restaurantDao.InsertRestaurantAsync(newRestaurant));
    }
}
