using Dal.Domain;
using Microsoft.AspNetCore.Mvc;
using static Dal.Domain.Order;

namespace Snacks.Api;

public class StatusInfo
{
    public static ProblemDetails InvalidMenuItemId(int menuItem_id) =>
    new ProblemDetails
    {
        Title = "Invalid menu item ID",
        Detail = $"menu item with ID {menuItem_id} does not exist"
    };

    public static ProblemDetails InvalidMenuItemRestaurantId(int restaurant_id) =>
    new ProblemDetails
    {
        Title = "Invalid menu item restaurant ID",
        Detail = $"restaurant with ID {restaurant_id} does not exist"
    };

    public static ProblemDetails InvalidDeliveryConditionId(int deliveryCondition_id) =>
    new ProblemDetails
    {
        Title = "Invalid delivery condition ID",
        Detail = $"delivery condition with ID {deliveryCondition_id} does not exist"
    };

    public static ProblemDetails InvalidDeliveryCondition() =>
    new ProblemDetails
    {
        Title = "Invalid delivery condition",
        Detail = "There might be a conflict with another delivery condition or " +
        "the restaurant might not exist. Please also check the range of distance and order value."
    };

    public static ProblemDetails InvalidOrderedItemsSelection(int restaurant_id) =>
    new ProblemDetails
    {
        Title = "Invalid selection of menu items for order",
        Detail = "The ordered items might not exist or do not belong to the restaurant {restaurant_id}."
    };

    public static ProblemDetails NoSuitableDeliveryCondition(int restaurant_id, int distance, decimal orderValue) =>
    new ProblemDetails
    {
        Title = "No suitable delivery condition",
        Detail = $"For restaurant {restaurant_id}, no suitable delivery condition was found " +
                 $"for distance {distance} and order value {orderValue}."
    };

    public static ProblemDetails RestaurantNotFound(int restaurant_id) =>
    new ProblemDetails
    {
        Title = "Restaurant not found",
        Detail = $"Restaurant with id {restaurant_id} was not found."
    };

    public static ProblemDetails OpeningHoursNotFound(int openingHoursId, int restaurantId) =>
    new ProblemDetails
    {
        Title = "Opening hours not found",
        Detail = $"Opening hours with id {openingHoursId} for restaurant {restaurantId} was not found."
    };

    public static ProblemDetails ClosingDayNotFound(string weekDay, int restaurantId) =>
    new ProblemDetails
    {
        Title = "Closing day not found",
        Detail = $"Closing day {weekDay} for restaurant {restaurantId} was not found."
    };

    public static ProblemDetails OrderNotFound(int order_id) =>
    new ProblemDetails
    {
        Title = "Order not found",
        Detail = $"Order with id {order_id} was not found."
    };

    public static ProblemDetails OrderNotPlaced() =>
        new ProblemDetails
        {
            Title = "Order not placed",
            Detail = "Order could not be placed."
        };

    public static ProblemDetails AddressNotAdded() =>
        new ProblemDetails
        {
            Title = "Address not added",
            Detail = "Address could not be added."
        };

    public static ProblemDetails OrderNotBelongToRestaurant(int orderId, int restaurantId) =>
    new ProblemDetails
    {
        Title = "Order does not belong to restaurant",
        Detail = $"Order with id {orderId} does not belong to restaurant with id {restaurantId}."
    };

    public static ProblemDetails NoChangeOrderStatusLink(int orderId, OrderStatus newStatus) =>
    new ProblemDetails
    {
        Title = "No change order status link",
        Detail = $"No change order status link found for order with id {orderId} and new status {newStatus}."
    };

    public static ProblemDetails InvalidOrderItems() =>
    new ProblemDetails
    {
        Title = "Invalid order items",
        Detail = "Order items are invalid. Order might not belong to same restaurant or no delivery condition found"
    };

    public static ProblemDetails InvalidApiKeyToken(int restaurantId, string apiKeyToken) =>
    new ProblemDetails
    {
        Title = "Invalid API key token",
        Detail = $"API key token {apiKeyToken} is invalid for restaurant with id {restaurantId}."
    };

    public static ProblemDetails OrderNotUpdated(int orderId) =>
    new ProblemDetails
    {
        Title = "Order not updated",
        Detail = $"Order with id {orderId} could not be updated."
    };

    public static ProblemDetails InvalidToken() =>
    new ProblemDetails
    {
        Title = "Invalid token",
        Detail = "Token is invalid."
    };

    public static ProblemDetails MenuItemNotUpdated(int menuItemId) =>
    new ProblemDetails
    {
        Title = "Menu item not updated",
        Detail = $"Menu item with id {menuItemId} could not be updated."
    };


}
