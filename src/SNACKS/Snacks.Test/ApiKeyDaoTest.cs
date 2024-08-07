

using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Test;
public class ApiKeyDaoTest
{
    private readonly IApiKeyDao apiKeyDao;
    private readonly IRestaurantDao restaurantDao;

    public ApiKeyDaoTest()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        apiKeyDao = new ApiKeyDao(connectionFactory);
        restaurantDao = new RestaurantDao(connectionFactory);
    }

    [Fact]
    public async Task InsertApiKeyAsync_Should_Insert_ApiKey()
    {
        var newRestaurant = new Restaurant(0, "Test Restaurant", "http://test.com", "test.jpg", 1);
        var newRestaurantId = await restaurantDao.InsertRestaurantAsync(newRestaurant);

        ApiKey apiKey = new ApiKey(newRestaurantId, "testKey");
        await apiKeyDao.InsertApiKeyAsync(apiKey);

        ApiKey? result_ApiKey = await apiKeyDao.GetApiKeyByRestaurantIdAsync(newRestaurantId);
        Assert.NotNull(result_ApiKey);
        Assert.Equal(result_ApiKey.ApiKeyValue, apiKey.ApiKeyValue);

    }

    [Fact]
    public async Task GetApiKeyByRestaurantIdAsync_Should_Work()
    {
        ApiKey? result_ApiKey = await apiKeyDao.GetApiKeyByRestaurantIdAsync(1);
        Assert.NotNull(result_ApiKey);
        Assert.Equal(result_ApiKey.ApiKeyValue, "testApiKey123"); // value comes from insert script
    }

}
