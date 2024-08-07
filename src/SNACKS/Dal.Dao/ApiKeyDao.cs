using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;

public class ApiKeyDao : IApiKeyDao
{
    private readonly AdoTemplate template;

    public ApiKeyDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    public ApiKey MapRowToApiKey(IDataRecord row)
    {
        return new ApiKey(
               (int)row["restaurant_id"],
               (string)row["api_key_value"],
               (DateTime)row["created_at"]
        );
    }

    public async Task<bool> DeleteApiKeyAsync(int restaurantId)
    {
        return await template.ExecuteAsync("DELETE FROM api_key WHERE restaurant_id = @restaurant_id", 
                                            new QueryParameter("@restaurant_id", restaurantId)) == 1;
    }

    public async Task<ApiKey?> GetApiKeyByRestaurantIdAsync(int restaurantId)
    {
        const string sql = "SELECT * FROM api_key WHERE restaurant_id = @restaurant_id";

        return await template.QuerySingleAsync(sql, MapRowToApiKey, new QueryParameter("@restaurant_id", restaurantId));
    }


    public async Task<int> InsertApiKeyAsync(ApiKey newApiKey)
    {
        const string sql = "INSERT INTO api_key (restaurant_id, api_key_value) " +
                           "VALUES (@restaurant_id, @api_key_value);" +
                           "SELECT LAST_INSERT_ID();";
        
        return await template.ExecuteScalarAsync<int>(sql,
                new QueryParameter("@restaurant_id", newApiKey.RestaurantId),
                new QueryParameter("@api_key_value", newApiKey.ApiKeyValue)
        );
    }
}
