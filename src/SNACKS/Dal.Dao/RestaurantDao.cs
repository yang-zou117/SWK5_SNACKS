using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;

public class RestaurantDao : IRestaurantDao
{
    private readonly AdoTemplate template;

    private Restaurant MapRowToRestaurant(IDataRecord row)
    {
        string? imagePath = row["image_path"] == DBNull.Value ? 
                            null : (string)row["image_path"];

        return new Restaurant(
               (int)row["restaurant_id"],
               (string)row["restaurant_name"],
               (string)row["webhook_url"],
               imagePath,
               (int)row["address_id"]
        );
    }

    private RestaurantDistanceResult MapRowToRestaurantDistanceResult(IDataRecord row)
    {
        string? imagePath = row["image_path"] == DBNull.Value ?
                            null : (string)row["image_path"];

        return new RestaurantDistanceResult(
               (int)row["restaurant_id"],
               (string)row["restaurant_name"],
               (string)row["webhook_url"],
               imagePath,
               (int)row["address_id"],
               Convert.ToDecimal((double)row["distance"])
        );
    }


    public RestaurantDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    public async Task<int> InsertRestaurantAsync(Restaurant newRestaurant)
    {
        const string sql = "INSERT INTO restaurant (restaurant_name, webhook_url, image_path, address_id) " +
                           "VALUES (@restaurant_name, @webhook_url, @image_path, @address_id);" +
                           "SELECT LAST_INSERT_ID();";

        return await template.ExecuteScalarAsync<int>(sql, new QueryParameter("@restaurant_name", newRestaurant.RestaurantName),
                                                           new QueryParameter("@webhook_url", newRestaurant.WebhookUrl),
                                                           new QueryParameter("@image_path", newRestaurant.ImagePath),
                                                           new QueryParameter("@address_id", newRestaurant.AddressId));
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int restaurantId)
    {
        const string sql = "SELECT * FROM restaurant WHERE restaurant_id = @restaurant_id";

        return await template.QuerySingleAsync(sql, MapRowToRestaurant, new QueryParameter("@restaurant_id", restaurantId));
    }

    public async Task<bool> DeleteRestaurantAsync(int restaurantId)
    {
        const string sql = "DELETE FROM restaurant WHERE restaurant_id = @restaurant_id";

        return await template.ExecuteAsync(sql, new QueryParameter("@restaurant_id", restaurantId)) == 1;
    }

    public async Task<bool> UpdateRestaurantAsync(Restaurant restaurant)
    {
        const string sql = "UPDATE restaurant SET restaurant_name = @restaurant_name, " +
                           "webhook_url = @webhook_url, image_path = @image_path, address_id = @address_id " +
                           "WHERE restaurant_id = @restaurant_id";

        return await template.ExecuteAsync(sql, new QueryParameter("@restaurant_id", restaurant.RestaurantId),
                                                new QueryParameter("@restaurant_name", restaurant.RestaurantName),
                                                new QueryParameter("@webhook_url", restaurant.WebhookUrl),
                                                new QueryParameter("@image_path", restaurant.ImagePath),
                                                new QueryParameter("@address_id", restaurant.AddressId)) == 1;
    }

    public Task<IEnumerable<RestaurantDistanceResult>> GetRestaurantsInProximityAsync(double latitude, double longitude, 
                                                                                      int maxDistance, bool shouldBeOpen)
    {
        // use the Haversine formula to calculate distance between two points on a sphere
        string sql; 
        if(shouldBeOpen)
        {
            sql =   "SELECT DISTINCT restaurant.restaurant_id, restaurant_name, webhook_url, image_path, restaurant.address_id, " +
                            "ROUND(ST_Distance_Sphere(POINT(@longitude, @latitude), " +
                                                      "POINT(address.gps_longitude, address.gps_latitude)) / 1000.0, 2) AS distance " +
                    "FROM restaurant INNER JOIN address ON restaurant.address_id = address.address_id " +
                          "INNER JOIN opening_hours ON restaurant.restaurant_id = opening_hours.restaurant_id " +
                    "WHERE ROUND(ST_Distance_Sphere(POINT(@longitude, @latitude), " +
                                                    "POINT(address.gps_longitude, address.gps_latitude)) / 1000) < @maxDistance AND " +
                          "opening_hours.week_day = DAYNAME(CURDATE()) AND " +
                          "opening_hours.start_time <= CURTIME() AND " +
                          "opening_hours.end_time >= CURTIME() " +
                    "ORDER BY distance ASC ;";

        }
        else 
        {

            sql =   "SELECT restaurant.restaurant_id, restaurant_name, webhook_url, image_path, restaurant.address_id, " +
                            "ROUND(ST_Distance_Sphere(POINT(@longitude, @latitude), " +
                                                      "POINT(address.gps_longitude, address.gps_latitude)) / 1000, 2) AS distance " +
                    "FROM restaurant INNER JOIN address ON restaurant.address_id = address.address_id " +
                    "WHERE ROUND(ST_Distance_Sphere(POINT(@longitude, @latitude), " +
                                                    "POINT(address.gps_longitude, address.gps_latitude)) / 1000) < @maxDistance " +
                    "ORDER BY distance ASC; ";

        }

        return template.QueryAsync(sql, MapRowToRestaurantDistanceResult, 
                                    new QueryParameter("@latitude", latitude),
                                    new QueryParameter("@longitude", longitude),
                                    new QueryParameter("@maxDistance", maxDistance));

    }
}
