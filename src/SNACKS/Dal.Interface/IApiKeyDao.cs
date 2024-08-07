using Dal.Domain;

namespace Dal.Interface;
public interface IApiKeyDao
{
    Task<ApiKey?> GetApiKeyByRestaurantIdAsync(int restaurantId);
    Task<int> InsertApiKeyAsync(ApiKey newApiKey);
    Task<bool> DeleteApiKeyAsync(int restaurantId);
}
