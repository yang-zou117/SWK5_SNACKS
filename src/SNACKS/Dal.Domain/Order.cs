namespace Dal.Domain;
public class Order
{
    public enum OrderStatus
    {
        Pending,
        BeingPrepared,
        InDelivery,
        Delivered,
        Cancelled
    }

    public Order(int orderId, int restaurantId, int addressId, 
                 string customerName, string phoneNumber, 
                 DateTime orderedDate, OrderStatus status, decimal orderCosts)
    {
        OrderId = orderId;
        RestaurantId = restaurantId;
        AddressId = addressId;
        CustomerName = customerName;
        PhoneNumber = phoneNumber;
        OrderedDate = orderedDate;
        Status = status;
        OrderCosts = orderCosts;
    }

    public Order()
    {
        
    }

    public int OrderId { get; set; }
    public int RestaurantId { get; set; }
    public int AddressId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime OrderedDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal OrderCosts { get; set; }


    public override bool Equals(object? obj)
    {
        if (obj is not Order other)
            return false;
        var order = (Order)obj;
        return OrderId == order.OrderId &&
               RestaurantId == order.RestaurantId &&
               AddressId == order.AddressId &&
               CustomerName == order.CustomerName &&
               PhoneNumber == order.PhoneNumber &&
               OrderedDate.Equals(OrderedDate) &&
               Status == order.Status &&
               OrderCosts == order.OrderCosts;
    }
}
