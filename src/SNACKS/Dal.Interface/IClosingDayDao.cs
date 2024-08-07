using Dal.Domain;

namespace Dal.Interface;

public interface IClosingDayDao
{
    Task<int> InsertClosingDayAsync(ClosingDay newClosingDay);
    Task<IEnumerable<ClosingDay>> GetClosingDaysForRestaurantAsync(int restaurantId);
    Task<bool> DeleteClosingDayAsync(string weekDay, int restaurantId);
}
