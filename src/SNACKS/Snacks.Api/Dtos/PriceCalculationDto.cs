using System.ComponentModel.DataAnnotations;

namespace Snacks.Api.Dtos;

public record PriceCalculationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid restaurant id")]
    public required int RestaurantId { get; init; }
    public required AddressForCreationDto DeliveryAddress { get; init; }
    public required OrderItemForCreationDto[] OrderedItems { get; init; }
}
