namespace Dal.Domain;

public class OpeningHours
{
    public OpeningHours(int openingHoursId, 
                        string weekDay, 
                        int restaurantId, 
                        TimeSpan startTime, 
                        TimeSpan endTime)
    {
        OpeningHoursId = openingHoursId;
        WeekDay = weekDay;
        RestaurantId = restaurantId;
        StartTime = startTime;
        EndTime = endTime;
    }

    public OpeningHours()
    {

    }

    public int OpeningHoursId { get; set; }
    public string WeekDay { get; set; }
    public int RestaurantId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public override string ToString() =>
        $"OpeningHours(OpeningHoursId: {OpeningHoursId}, " +
        $"WeekDay: {WeekDay}, " +
        $"RestaurantId: {RestaurantId}, " +
        $"StartTime: {StartTime:HH:mm}, " +
        $"EndTime: {EndTime:HH:mm})";

    public override bool Equals(object? obj) =>
        obj is OpeningHours other &&
        OpeningHoursId == other.OpeningHoursId &&
        WeekDay == other.WeekDay &&
        RestaurantId == other.RestaurantId &&
        StartTime == other.StartTime &&
        EndTime == other.EndTime;
}