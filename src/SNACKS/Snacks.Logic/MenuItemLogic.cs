using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Logic;
public class MenuItemLogic : IMenuItemLogic
{
    private IMenuItemDao menuItemDao;
    private IRestaurantDao restaurantDao;

    public MenuItemLogic()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        menuItemDao = new MenuItemDao(connectionFactory);
        restaurantDao = new RestaurantDao(connectionFactory);
    }
    
    public async Task<int> AddMenuItemAsync(MenuItem item)
    {
        return await menuItemDao.InsertMenuItemAsync(item);
    }

    public async Task<bool> DeleteMenuItemAsync(int menuItemId)
    {
        return await menuItemDao.DeleteMenuItemAsync(menuItemId);
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
    {
        return await menuItemDao.GetMenuItemByIdAsync(menuItemId);
    }

    public async Task<bool> UpdateMenuItemAsync(MenuItem item)
    {
        return await menuItemDao.UpdateMenuItemAsync(item);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantIdAsync(int restaurantId)
    {
        return await menuItemDao.GetMenuItemsForRestaurantAsync(restaurantId);
    }
}
