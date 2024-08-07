using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Logic;
public class ApiKeyLogic : IApiKeyLogic
{
    private IApiKeyDao apiKeyDao;

    public ApiKeyLogic()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        apiKeyDao = new ApiKeyDao(connectionFactory);
    }

    public async Task<ApiKey?> GetApiKeyForRestaurantAsync(int restaurant_id)
    {
        return await apiKeyDao.GetApiKeyByRestaurantIdAsync(restaurant_id);
    }

    public async Task<int> InsertApiKeyAsync(ApiKey new_api_key)
    {
        return await apiKeyDao.InsertApiKeyAsync(new_api_key);
    }

    public async Task<bool> ValidateApiKeyAsync(ApiKey api_key_to_validate)
    {
        var api_key = 
            await apiKeyDao.GetApiKeyByRestaurantIdAsync(api_key_to_validate.RestaurantId);
        
        if (api_key == null)
            return false;
        
        return api_key.ApiKeyValue == api_key_to_validate.ApiKeyValue;
    }
}
