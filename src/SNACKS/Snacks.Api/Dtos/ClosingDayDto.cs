namespace Snacks.Api.Dtos;

public record ClosingDayDto
{
    public required string WeekDay { get; set; }
    public required int RestaurantId { get; set; }
}
