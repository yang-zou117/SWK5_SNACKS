using System.ComponentModel.DataAnnotations;

namespace Snacks.Api.Dtos;

public record DeliveryConditionForUpdateDto
{
    [Range(0, int.MaxValue, ErrorMessage = "distance_lower_threshold not valid")]
    public required int DistanceLowerThreshold { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "distance_upper_threshold not valid")]
    public required int DistanceUpperThreshold { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "delivery_cost not valid")]
    public required decimal OrderValueLowerThreshold { get; set; }
    
    public decimal? OrderValueUpperThreshold { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "delivery_cost not valid")]
    public required decimal DeliveryCosts { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "min_order_value not valid")]
    public required decimal MinOrderValue { get; set; }
}
