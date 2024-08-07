namespace Snacks.Api.Dtos;

public record OpeningHoursDto
{
    public required int OpeningHoursId { get; set; }
    public required string WeekDay { get; set; }
    public required int RestaurantId { get; set; }
    public required TimeSpan StartTime { get; set; }
    public required TimeSpan EndTime { get; set; }
}
