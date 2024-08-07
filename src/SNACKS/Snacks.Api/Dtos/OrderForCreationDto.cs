using System.ComponentModel.DataAnnotations;

namespace Snacks.Api.Dtos;

public record OrderForCreationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid restaurant id")]
    public required int RestaurantId { get; set; }

    public required string CustomerName { get; set; }

    public required string PhoneNumber { get; set; }

}
