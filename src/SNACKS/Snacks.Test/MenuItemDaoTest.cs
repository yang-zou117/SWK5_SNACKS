using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Test;

public class MenuItemDaoTests
{
    private readonly IMenuItemDao menuItemDao;

    public MenuItemDaoTests()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        menuItemDao = new MenuItemDao(connectionFactory);
    }

    [Fact]
    public async Task InsertMenuItem_WhenCalled_ShouldInsertMenuItem()
    {
        var newItem = new MenuItem(1, 1, "Test Item", "Test Description", 1.99m, "Test Category");
        var newMenuItemId = await menuItemDao.InsertMenuItemAsync(newItem);
        newItem.MenuItemId = newMenuItemId;

        Assert.True(newMenuItemId > 0);
        Assert.Equal(newItem, await menuItemDao.GetMenuItemByIdAsync(newMenuItemId));
    }

    [Fact]
    public async Task UpdateMenuItem_WhenCalled_ShouldUpdateMenuItem()
    {
        var newItem = new MenuItem(1, 1, "Test Item", "Test Description", 1.99m, "Test Category");
        var newMenuItemId = await menuItemDao.InsertMenuItemAsync(newItem);
        newItem.MenuItemId = newMenuItemId;
        
        newItem.MenuItemName = "Updated Item";
        newItem.MenuItemDescription = "Updated Description";
        newItem.Price = 2.99m;
        newItem.CategoryName = "Updated Category";

        Assert.True(await menuItemDao.UpdateMenuItemAsync(newItem));
        Assert.Equal(newItem, await menuItemDao.GetMenuItemByIdAsync(newMenuItemId));
    }

    [Fact]
    public async Task DeleteMenuItem_WhenCalled_ShouldDeleteMenuItem()
    {
        var newItem = new MenuItem(1, 1, "Test Item", "Test Description", 1.99m, "Test Category");
        var newMenuItemId = await menuItemDao.InsertMenuItemAsync(newItem);
        newItem.MenuItemId = newMenuItemId;

        Assert.True(await menuItemDao.DeleteMenuItemAsync(newMenuItemId));
        Assert.True(await menuItemDao.GetMenuItemByIdAsync(newMenuItemId) == null);
    }

    [Fact]
    public async Task GetMenuItemsForRestaurant_Test()
    {
        var newItem = new MenuItem(1, 1, "Test Item", "Test Description", 1.99m, "Test Category");
        var newMenuItemId = await menuItemDao.InsertMenuItemAsync(newItem);
        newItem.MenuItemId = newMenuItemId;

        var menuItems = await menuItemDao.GetMenuItemsForRestaurantAsync(1);
        Assert.NotNull(menuItems);
        var enumerable = menuItems as MenuItem[] ?? menuItems.ToArray();
        Assert.True(enumerable.ToList().Count > 0);
        Assert.Contains(newItem, enumerable.ToList());
    }



}