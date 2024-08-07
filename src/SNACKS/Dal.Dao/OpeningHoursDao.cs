using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;

public class OpeningHoursDao: IOpeningHoursDao
{
    private readonly AdoTemplate template;

    public OpeningHoursDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    private OpeningHours MapRowToOpeningHours(IDataRecord row)
    {
        return new OpeningHours(
        (int)row["opening_hours_id"],
        (string)row["week_day"],
        (int)row["restaurant_id"],
        (TimeSpan)row["start_time"],
        (TimeSpan)row["end_time"]
                                                                          );
    }   

    public async Task<bool> DeleteOpeningHoursAsync(int openingHoursId)
    {
        const string sql = "DELETE FROM opening_hours WHERE opening_hours_id = @opening_hours_id";
        return await template.ExecuteAsync(sql, new QueryParameter("@opening_hours_id", openingHoursId)) == 1;
    }

    public async Task<IEnumerable<OpeningHours>> GetOpeningHoursForRestaurantAsync(int restaurant_id)
    {
        const string sql = "SELECT * FROM opening_hours WHERE restaurant_id = @restaurant_id";

        return await template.QueryAsync(sql, MapRowToOpeningHours,
                                         new QueryParameter("@restaurant_id", restaurant_id));
    }

    public async Task<int> InsertOpeningHoursAsync(OpeningHours newOpeningHours)
    {
        const string sql = "INSERT INTO opening_hours (restaurant_id, week_day, start_time, end_time) " +
                           "VALUES (@restaurant_id, @week_day, @start_time, @end_time);" +
                           "SELECT LAST_INSERT_ID();";

        return await
            template.ExecuteScalarAsync<int>(sql, new QueryParameter("@restaurant_id", newOpeningHours.RestaurantId),
                                                  new QueryParameter("@week_day", newOpeningHours.WeekDay),
                                                  new QueryParameter("@start_time", newOpeningHours.StartTime),
                                                  new QueryParameter("@end_time", newOpeningHours.EndTime));
    }

    public async Task<bool> UpdateOpeningHoursAsync(OpeningHours openingHours)
    {
        const string sql = "UPDATE opening_hours SET week_day = @week_day, " +
                           "start_time = @start_time, end_time = @end_time " +
                           "WHERE opening_hours_id = @opening_hours_id";

        return await template.ExecuteAsync(sql, new QueryParameter("@opening_hours_id", openingHours.OpeningHoursId),
                                                new QueryParameter("@week_day", openingHours.WeekDay),
                                                new QueryParameter("@start_time", openingHours.StartTime),
                                                new QueryParameter("@end_time", openingHours.EndTime)) == 1;
    }
}
