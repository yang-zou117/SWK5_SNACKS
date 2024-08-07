using Dal.Domain;

namespace Dal.Interface; 

public interface IRestaurantDao
{
    Task<int> InsertRestaurantAsync(Restaurant newRestaurant);
    Task<Restaurant?> GetRestaurantByIdAsync(int restaurantId);
    Task<bool> DeleteRestaurantAsync(int restaurantId);
    Task<bool> UpdateRestaurantAsync(Restaurant restaurant);
    Task<IEnumerable<RestaurantDistanceResult>> GetRestaurantsInProximityAsync(double latitude, double longitude, int maxDistance, bool shouldBeOpen);
}
