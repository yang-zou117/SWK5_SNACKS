using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Test;

public class OpeningHoursTest
{
    private readonly IOpeningHoursDao openingsHoursDao;

    public OpeningHoursTest()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        openingsHoursDao = new OpeningHoursDao(connectionFactory);
    }

    [Fact]
    public async Task InsertOpeningHoursAsyncTest()
    {
        var openingHours = new OpeningHours(0, "Wednesday", 1, 
                                            new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0));

        var result = await openingsHoursDao.InsertOpeningHoursAsync(openingHours);
        Assert.True(result > 0);
    }

    [Fact]
    public async Task GetOpeningHoursForRestaurantAsyncTest()
    {
        var openingHours = new OpeningHours(0, "Wednesday", 1,
                                    new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0));

        var openingHoursId = await openingsHoursDao.InsertOpeningHoursAsync(openingHours);
        openingHours.OpeningHoursId = openingHoursId;
        var result = await openingsHoursDao.GetOpeningHoursForRestaurantAsync(1);
        Assert.NotNull(result);
        Assert.Contains(openingHours, result);
    }

    [Fact]
    public async Task DeleteOpeningHoursAsyncTest()
    {
        var openingHours = new OpeningHours(0, "Wednesday", 1,
                                    new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0));

        var openingHoursId = await openingsHoursDao.InsertOpeningHoursAsync(openingHours);
        Assert.True(await openingsHoursDao.DeleteOpeningHoursAsync(openingHoursId));
    }

    [Fact]
    public async Task UpdateOpeningHoursAsyncTest()
    {
        var openingHours = new OpeningHours(0, "Wednesday", 1,
                                    new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0));

        var openingHoursId = await openingsHoursDao.InsertOpeningHoursAsync(openingHours);
        openingHours.OpeningHoursId = openingHoursId;
        openingHours.WeekDay = "Tuesday";
        Assert.True(await openingsHoursDao.UpdateOpeningHoursAsync(openingHours));
    }
}
