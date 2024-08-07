using Dal.Domain;

namespace Snacks.Api.Dtos;

public record OrderForExternalApiDto
{
    public required Order Order { get; init; }
    public required AddressForCreationDto DeliveryAddress { get; set; }
    public required OrderItemForCreationDto[] OrderedItems { get; set; }
}
