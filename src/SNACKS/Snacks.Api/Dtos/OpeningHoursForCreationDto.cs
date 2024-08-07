namespace Snacks.Api.Dtos;

public record OpeningHoursForCreationDto
{
    public required string WeekDay { get; set; }
    public required TimeSpan StartTime { get; set; }
    public required TimeSpan EndTime { get; set; }
}
