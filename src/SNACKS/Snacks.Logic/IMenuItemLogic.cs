using Dal.Domain;

namespace Snacks.Logic;

public interface IMenuItemLogic
{
    Task<int> AddMenuItemAsync(MenuItem item);
    Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
    Task<bool> UpdateMenuItemAsync(MenuItem item);
    Task<bool> DeleteMenuItemAsync(int menuItemId);
    Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantIdAsync(int restaurantId);
}
