namespace Dal.Domain;

public class ApiKey
{
    public ApiKey(int restaurantId, string apiKeyValue, DateTime createdAt)
    {
        this.RestaurantId = restaurantId;
        this.ApiKeyValue = apiKeyValue;
        this.CreatedAt = createdAt;
    }

    public ApiKey(int restaurantId, string apiKeyValue)
    {
        this.RestaurantId = restaurantId;
        this.ApiKeyValue = apiKeyValue;
    }

    public ApiKey()
    {

    }

    public int RestaurantId { get; set; }
    public string ApiKeyValue { get; set; }

    public DateTime? CreatedAt { get; set; }

}
