using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;

public class ClosingDayDao : IClosingDayDao
{
    private readonly AdoTemplate template;

    public ClosingDayDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    public ClosingDay MapRowToClosingDay(IDataRecord row)
    {
        return new ClosingDay(
            (string)row["week_day"],
            (int)row["restaurant_id"]
        );
    }

    public async Task<bool> DeleteClosingDayAsync(string weekDay, int restaurantId)
    {
        const string sql = "DELETE FROM closing_day WHERE restaurant_id = @restaurant_id AND week_day = @week_day";
        return await template.ExecuteAsync(sql, 
            new QueryParameter("@restaurant_id", restaurantId),
            new QueryParameter("@week_day", weekDay)
        ) == 1;
    }

    public async Task<IEnumerable<ClosingDay>> GetClosingDaysForRestaurantAsync(int restaurantId)
    {
        const string sql = "SELECT * FROM closing_day WHERE restaurant_id = @restaurant_id";
        return await template.QueryAsync(sql, MapRowToClosingDay,
                                         new QueryParameter("@restaurant_id", restaurantId));
    }

    public async Task<int> InsertClosingDayAsync(ClosingDay newClosingDay)
    {
        const string sql = "INSERT INTO closing_day (restaurant_id, week_day) " +
                           "VALUES (@restaurant_id, @week_day);" +
                           "SELECT LAST_INSERT_ID();";

        return await
            template.ExecuteScalarAsync<int>(sql, new QueryParameter("@restaurant_id", newClosingDay.RestaurantId),
                                                  new QueryParameter("@week_day", newClosingDay.WeekDay));
    }
}
