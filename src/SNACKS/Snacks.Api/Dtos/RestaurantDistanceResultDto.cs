namespace Snacks.Api.Dtos;

public record RestaurantDistanceResultDto
{
    public required RestaurantDto Restaurant { get; init; }
    public required decimal Distance { get; init; }
}
