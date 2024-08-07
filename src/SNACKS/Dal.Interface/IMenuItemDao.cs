using Dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Interface;

public interface IMenuItemDao
{
    Task<int> InsertMenuItemAsync(MenuItem newItem);
    Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId);
    Task<IEnumerable<MenuItem>> GetMenuItemsForRestaurantAsync(int restaurantId);
    Task<bool> UpdateMenuItemAsync(MenuItem item);
    Task<bool> DeleteMenuItemAsync(int menuItemId);
}
