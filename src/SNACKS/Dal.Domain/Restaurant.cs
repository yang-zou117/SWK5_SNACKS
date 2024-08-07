namespace Dal.Domain;

public class Restaurant
{
    public Restaurant(int restaurantId, 
                      string restaurantName, 
                      string webhookUrl, 
                      string? imagePath, 
                      int addressId)
    {
        RestaurantId = restaurantId;
        RestaurantName = restaurantName ?? throw new ArgumentNullException(nameof(restaurantName));
        WebhookUrl = webhookUrl ?? throw new ArgumentNullException(nameof(webhookUrl));
        ImagePath = imagePath;
        AddressId = addressId;
    }
    
    public Restaurant()
    {

    }

    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public string WebhookUrl { get; set; }
    public string? ImagePath { get; set; }
    public int AddressId { get; set; }

    public override string ToString() =>
        $"Restaurant(RestaurantId: {RestaurantId}, " +
        $"RestaurantName: {RestaurantName}, " +
        $"WebhookUrl: {WebhookUrl}, " +
        $"ImagePath: {ImagePath}, " +
        $"AddressId: {AddressId})";

    public override bool Equals(object? obj)
    {
        return obj is Restaurant restaurant &&
               RestaurantId == restaurant.RestaurantId &&
               RestaurantName == restaurant.RestaurantName &&
               WebhookUrl == restaurant.WebhookUrl &&
               ImagePath == restaurant.ImagePath &&
               AddressId == restaurant.AddressId;
    }
}

