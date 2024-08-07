using static Dal.Domain.Order;

namespace Dal.Dao;
public static class Util
{
    public static OrderStatus ParseOrderStatus(string statusString)
    {
        if (Enum.TryParse(statusString, out OrderStatus status))
        {
            return status;
        }
        else
        {
            throw new ArgumentException($"Invalid OrderStatus value: {statusString}");
        }
    }
}
