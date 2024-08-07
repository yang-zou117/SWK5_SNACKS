using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;

public class DeliveryConditionDao : IDeliveryConditionDao
{
    private readonly AdoTemplate template;

    public DeliveryConditionDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    private DeliveryCondition MapRowToDeliveryCondition(IDataRecord row)
    {
        decimal? orderValueUpperThreshold = row["order_value_upper_threshold"] == DBNull.Value ?
            null : (decimal?)row["order_value_upper_threshold"];

        return new DeliveryCondition(
            (int)row["delivery_condition_id"],
            (int)row["restaurant_id"],
            (int)row["distance_lower_threshold"],
            (int)row["distance_upper_threshold"],
            (decimal)row["order_value_lower_threshold"],
            orderValueUpperThreshold,
            (decimal)row["delivery_costs"],
            (decimal)row["min_order_value"]
        );
    }

    
    public async Task<bool> DeleteDeliveryConditionAsync(int deliveryConditionId)
    {
        const string sql = "DELETE FROM delivery_condition WHERE delivery_condition_id = @delivery_condition_id";
        return await template.ExecuteAsync(sql, new QueryParameter("@delivery_condition_id", deliveryConditionId)) == 1;
    }

    public async Task<IEnumerable<DeliveryCondition>> GetDeliveryConditionsForRestaurantAsync(int restaurantId)
    {
        const string sql = "SELECT * FROM delivery_condition WHERE restaurant_id = @restaurant_id";

        return await template.QueryAsync(sql, MapRowToDeliveryCondition, 
                                         new QueryParameter("@restaurant_id", restaurantId));
    }

    public async Task<int> InsertDeliveryConditionAsync(DeliveryCondition newCondition)
    {
        const string sql = "INSERT INTO delivery_condition (restaurant_id, distance_lower_threshold, distance_upper_threshold, " +
                           "order_value_lower_threshold, order_value_upper_threshold, delivery_costs, min_order_value)"+
                           "VALUES (@restaurant_id, @distance_lower_threshold, @distance_upper_threshold, " +
                           "@order_value_lower_threshold, @order_value_upper_threshold, @delivery_costs, @min_order_value);" +
                           "SELECT LAST_INSERT_ID();";
        
        return await 
            template.ExecuteScalarAsync<int>(sql, new QueryParameter("@restaurant_id", newCondition.RestaurantId),
                                 new QueryParameter("@distance_lower_threshold", newCondition.DistanceLowerThreshold),
                                 new QueryParameter("@distance_upper_threshold", newCondition.DistanceUpperThreshold),
                                 new QueryParameter("@order_value_lower_threshold", newCondition.OrderValueLowerThreshold),
                                 new QueryParameter("@order_value_upper_threshold", newCondition.OrderValueUpperThreshold),
                                 new QueryParameter("@delivery_costs", newCondition.DeliveryCosts),
                                 new QueryParameter("@min_order_value", newCondition.MinOrderValue));
    }

    public async Task<bool> UpdateDeliveryConditionAsync(DeliveryCondition condition)
    {
        const string sql = "UPDATE delivery_condition SET distance_lower_threshold = @distance_lower_threshold, " +
                           "distance_upper_threshold = @distance_upper_threshold, order_value_lower_threshold = @order_value_lower_threshold, " +
                           "order_value_upper_threshold = @order_value_upper_threshold, delivery_costs = @delivery_costs, " +
                           "min_order_value = @min_order_value WHERE delivery_condition_id = @delivery_condition_id";

        return await template.ExecuteAsync(sql, new QueryParameter("@distance_lower_threshold", condition.DistanceLowerThreshold),
                                 new QueryParameter("@distance_upper_threshold", condition.DistanceUpperThreshold),
                                 new QueryParameter("@order_value_lower_threshold", condition.OrderValueLowerThreshold),
                                 new QueryParameter("@order_value_upper_threshold", condition.OrderValueUpperThreshold),
                                 new QueryParameter("@delivery_costs", condition.DeliveryCosts),
                                 new QueryParameter("@min_order_value", condition.MinOrderValue),
                                 new QueryParameter("@delivery_condition_id", condition.DeliveryConditionId)) == 1;
    }

    public async Task<DeliveryCondition?> GetDeliveryConditionByIdAsync(int deliveryConditionId)
    {
        const string sql = "SELECT * FROM delivery_condition WHERE delivery_condition_id = @delivery_condition_id";
        return await template.QuerySingleAsync(sql, MapRowToDeliveryCondition,
                                               new QueryParameter("@delivery_condition_id", deliveryConditionId));
    }
}
