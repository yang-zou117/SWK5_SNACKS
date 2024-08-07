namespace Snacks.Api.Dtos;

public record RegisterDto
{
    public required RestaurantForCreationDto Restaurant { get; set; }
    public required AddressForCreationDto Address { get; set; }
    public required OpeningHoursForCreationDto[] OpeningHours { get; set; }
    public required ClosingDayForCreationDto[] ClosingDays { get; set; }

}
