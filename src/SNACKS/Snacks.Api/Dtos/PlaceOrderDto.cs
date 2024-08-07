namespace Snacks.Api.Dtos;

public record PlaceOrderDto
{
    public required OrderForCreationDto Order { get; set; }
    public required AddressForCreationDto DeliveryAddress { get; set; }
    public required OrderItemForCreationDto[] OrderedItems { get; set; }

}
