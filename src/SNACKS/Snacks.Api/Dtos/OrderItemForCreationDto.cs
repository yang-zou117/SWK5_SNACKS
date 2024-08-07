using System.ComponentModel.DataAnnotations;

namespace Snacks.Api.Dtos;

public record OrderItemForCreationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Menu_item_id invalid")]
    public required int MenuItemId { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "The amount must be a positive integer.")]
    public required int Amount { get; init; }
}
