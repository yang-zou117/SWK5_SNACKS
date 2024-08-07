namespace Dal.Domain;

public class DeliveryCondition
{
    public DeliveryCondition(
        int deliveryConditionId,
        int restaurantId,
        int distanceLowerThreshold,
        int distanceUpperThreshold,
        decimal orderValueLowerThreshold,
        decimal? orderValueUpperThreshold,
        decimal deliveryCosts,
        decimal minOrderValue)
    {
        DeliveryConditionId = deliveryConditionId;
        RestaurantId = restaurantId;
        DistanceLowerThreshold = distanceLowerThreshold;
        DistanceUpperThreshold = distanceUpperThreshold;
        OrderValueLowerThreshold = orderValueLowerThreshold;
        OrderValueUpperThreshold = orderValueUpperThreshold;
        DeliveryCosts = deliveryCosts;
        MinOrderValue = minOrderValue;
    }

    public DeliveryCondition()
    {

    }

    public int DeliveryConditionId { get; set; }
    public int RestaurantId { get; set; }
    public int DistanceLowerThreshold { get; set; }
    public int DistanceUpperThreshold { get; set; }
    public decimal OrderValueLowerThreshold { get; set; }
    public decimal? OrderValueUpperThreshold { get; set; }
    public decimal DeliveryCosts { get; set; }
    public decimal MinOrderValue { get; set; }

    public override string ToString() =>
        $"DeliveryConditions(DeliveryConditionId: {DeliveryConditionId}, " +
        $"RestaurantId: {RestaurantId}, " +
        $"DistanceLowerThreshold: {DistanceLowerThreshold}, " +
        $"DistanceUpperThreshold: {DistanceUpperThreshold}, " +
        $"OrderValueLowerThreshold: {OrderValueLowerThreshold}, " +
        $"OrderValueUpperThreshold: {OrderValueUpperThreshold}, " +
        $"DeliveryCosts: {DeliveryCosts:C}, " +
        $"MinOrderValue: {MinOrderValue:C})";

    public override bool Equals(object? obj) =>
        obj is DeliveryCondition condition &&
        DeliveryConditionId == condition.DeliveryConditionId &&
        RestaurantId == condition.RestaurantId &&
        DistanceLowerThreshold == condition.DistanceLowerThreshold &&
        DistanceUpperThreshold == condition.DistanceUpperThreshold &&
        OrderValueLowerThreshold == condition.OrderValueLowerThreshold &&
        OrderValueUpperThreshold == condition.OrderValueUpperThreshold &&
        DeliveryCosts == condition.DeliveryCosts &&
        MinOrderValue == condition.MinOrderValue;
}
