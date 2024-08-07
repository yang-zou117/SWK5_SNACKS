using Dal.Domain;

namespace Snacks.Logic;
public interface IApiKeyLogic
{

    Task<bool> ValidateApiKeyAsync(ApiKey api_key_to_validate);

    Task<int> InsertApiKeyAsync(ApiKey new_api_key);

    Task<ApiKey?> GetApiKeyForRestaurantAsync(int restaurant_id);

}
