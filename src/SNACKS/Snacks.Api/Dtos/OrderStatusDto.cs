using static Dal.Domain.Order;

namespace Snacks.Api.Dtos;

public record OrderStatusDto
{
    public required OrderStatus Status { get; set; }
}
