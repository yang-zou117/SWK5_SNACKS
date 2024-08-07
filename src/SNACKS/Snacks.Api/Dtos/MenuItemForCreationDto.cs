namespace Snacks.Api.Dtos;

public record MenuItemForCreationDto
{
    public required string MenuItemName { get; set; }

    public string MenuItemDescription { get; set; } = string.Empty;

    public required decimal Price { get; set; }

    public required string CategoryName { get; set; }
}
