using static Dal.Domain.Order;

namespace Dal.Domain;

public class ChangeOrderStatusLink
{
    public ChangeOrderStatusLink(string link, int orderId, Order.OrderStatus newStatus)
    {
        Link = link;
        OrderId = orderId;
        NewStatus = newStatus;
    }

    public ChangeOrderStatusLink()
    {
        
    }

    public string Link { get; set; }
    public int OrderId { get; set; }
    public Order.OrderStatus NewStatus { get; set; }
}
