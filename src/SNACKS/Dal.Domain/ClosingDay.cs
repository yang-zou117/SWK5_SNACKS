namespace Dal.Domain;

public class ClosingDay
{
    public ClosingDay(string weekDay, int restaurantId)
    {
        WeekDay = weekDay ?? throw new ArgumentNullException(nameof(weekDay));
        RestaurantId = restaurantId;
    }

    public ClosingDay()
    {

    }

    public string WeekDay { get; set; }
    public int RestaurantId { get; set; }

    public override string ToString() =>
        $"ClosingDay(WeekDay: {WeekDay}, RestaurantId: {RestaurantId})";

    public override bool Equals(object? obj) =>
        obj is ClosingDay closingDay &&
        WeekDay == closingDay.WeekDay &&
        RestaurantId == closingDay.RestaurantId;
}
