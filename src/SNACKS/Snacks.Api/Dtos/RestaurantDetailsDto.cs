namespace Snacks.Api.Dtos;

public record RestaurantDetailsDto
{
    public required RestaurantDto Restaurant { get; init; }
    public required AddressDto Address { get; init; }
    public required IEnumerable<OpeningHoursDto> OpeningHours { get; init; }
    public required IEnumerable<ClosingDayDto> ClosingDays { get; init; }
}
