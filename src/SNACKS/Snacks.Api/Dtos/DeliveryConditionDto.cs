namespace Snacks.Api.Dtos;

public record DeliveryConditionDto
{
    public required int DeliveryConditionId { get; set; }
    public required int RestaurantId { get; set; }
    public required int DistanceLowerThreshold { get; set; }
    public required int DistanceUpperThreshold { get; set; }
    public required decimal OrderValueLowerThreshold { get; set; }
    public decimal? OrderValueUpperThreshold { get; set; }
    public required decimal DeliveryCosts { get; set; }
    public required decimal MinOrderValue { get; set; }

}
