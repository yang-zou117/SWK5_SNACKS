namespace Dal.Domain;

public class MenuItem
{
    public MenuItem(int menuItemId, int restaurantId, 
                    string menuItemName, string? menuItemDescription, 
                    decimal price, string categoryName)
    {
        MenuItemId = menuItemId;
        RestaurantId = restaurantId;
        MenuItemName = menuItemName;
        MenuItemDescription = menuItemDescription;
        Price = price;
        CategoryName = categoryName;
    }

    public MenuItem()
    {

    }

    public int MenuItemId { get; set; }
    public int RestaurantId { get; set; }
    public string MenuItemName { get; set; }
    public string? MenuItemDescription { get; set; }
    public decimal Price { get; set; }
    public String CategoryName { get; set; }

    public override string ToString() =>
        $"MenuItem(MenuItemId: {MenuItemId}, RestaurantId: {RestaurantId}, " +
        $"MenuItemName: {MenuItemName}, MenuItemDescription: {MenuItemDescription}, " +
        $"Price: {Price:C}, Category: {CategoryName})";

    public override bool Equals(object? obj)
    {
        return obj is MenuItem item &&
               MenuItemId == item.MenuItemId &&
               RestaurantId == item.RestaurantId &&
               MenuItemName == item.MenuItemName &&
               MenuItemDescription == item.MenuItemDescription &&
               Price == item.Price &&
               CategoryName == item.CategoryName;
    }
}
