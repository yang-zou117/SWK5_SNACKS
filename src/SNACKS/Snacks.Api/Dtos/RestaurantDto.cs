namespace Snacks.Api.Dtos;

public record RestaurantDto
{
    public required int RestaurantId { get; set; }
    public required string RestaurantName { get; set; }
    public required string WebhookUrl { get; set; }
    public string? ImagePath { get; set; }
    public required int AddressId { get; set; }

}
