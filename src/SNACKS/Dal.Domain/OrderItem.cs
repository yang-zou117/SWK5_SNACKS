namespace Dal.Domain;

public class OrderItem
{
    public OrderItem(int orderItemId, int orderId, int menuItemId, int amount)
    {
        OrderItemId = orderItemId;
        OrderId = orderId;
        MenuItemId = menuItemId;
        Amount = amount;
    }

    public OrderItem()
    {
        
    }

    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public int Amount { get; set; }
}
