namespace Dal.Domain;

public class RestaurantDistanceResult
{
    public RestaurantDistanceResult(int restaurantId, 
                                    string restaurantName, 
                                    string webhookUrl, 
                                    string? imagePath, 
                                    int addressId, 
                                    decimal distance)
    {
        Distance = distance;
        Restaurant = new Restaurant(restaurantId, restaurantName, webhookUrl, imagePath, addressId);

    }

    public RestaurantDistanceResult()
    {

    }

    public Restaurant Restaurant { get; set; }
    public decimal Distance { get; set; }
}
