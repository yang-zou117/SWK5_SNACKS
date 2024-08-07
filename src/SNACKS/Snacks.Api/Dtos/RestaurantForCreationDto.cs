namespace Snacks.Api.Dtos;

public record RestaurantForCreationDto
{
    public required string RestaurantName { get; set; }
    public required string WebhookUrl { get; set; }
    public ImageRecord? Image { get; set; }
}
