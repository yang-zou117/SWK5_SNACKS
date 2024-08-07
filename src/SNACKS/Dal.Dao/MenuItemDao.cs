using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;

public class MenuItemDao : IMenuItemDao
{
    private readonly AdoTemplate template;

    public MenuItemDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    private MenuItem MapRowToMenuItem(IDataRecord row)
    {
        string? menuItemDescription = row["menu_item_description"] == DBNull.Value ? 
                                      null : (string)row["menu_item_description"];

        return new MenuItem(
            (int)row["menu_item_id"],
            (int)row["restaurant_id"],
            (string)row["menu_item_name"],
            menuItemDescription,
            (decimal)row["price"],
            (string)row["category_name"]
        );
    }


    public async Task<int> InsertMenuItemAsync(MenuItem newItem)
    {
        const string sql = "INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name) " +
                           "VALUES (@restaurant_id, @menu_item_name, @menu_item_description, @price, @category_name);" +
                           "SELECT LAST_INSERT_ID();";

        return await 
            template.ExecuteScalarAsync<int>(sql, new QueryParameter("@restaurant_id", newItem.RestaurantId),
                                                  new QueryParameter("@menu_item_name", newItem.MenuItemName),
                                                  new QueryParameter("@menu_item_description", newItem.MenuItemDescription),
                                                  new QueryParameter("@price", newItem.Price),
                                                  new QueryParameter("@category_name", newItem.CategoryName));
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsForRestaurantAsync(int restaurantId)
    {
        const string sql = "SELECT * FROM menu_item WHERE restaurant_id = @restaurant_id";

        return await template.QueryAsync(sql, MapRowToMenuItem, new QueryParameter("@restaurant_id", restaurantId));
    }

    public async Task<bool> UpdateMenuItemAsync(MenuItem item)
    {
        const string sql = "UPDATE menu_item SET menu_item_name = @menu_item_name, " +
                           "menu_item_description = @menu_item_description, price = @price, " +
                           "category_name = @category_name " +
                           "WHERE menu_item_id = @menu_item_id";

        return await template.ExecuteAsync(sql, new QueryParameter("@menu_item_id", item.MenuItemId),
                                                new QueryParameter("@menu_item_name", item.MenuItemName),
                                                new QueryParameter("@menu_item_description", item.MenuItemDescription),
                                                new QueryParameter("@price", item.Price),
                                                new QueryParameter("@category_name", item.CategoryName)) == 1;
    }

    public async Task<bool> DeleteMenuItemAsync(int menuItemId)
    {
        const string sql = "DELETE FROM menu_item WHERE menu_item_id = @menu_item_id";

        return await template.ExecuteAsync(sql, new QueryParameter("@menu_item_id", menuItemId)) == 1;
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int menuItemId)
    {
        const string sql = "SELECT * FROM menu_item WHERE menu_item_id = @menu_item_id";

        return await template.QuerySingleAsync(sql, MapRowToMenuItem, 
                                               new QueryParameter("@menu_item_id", menuItemId));
    }
}
