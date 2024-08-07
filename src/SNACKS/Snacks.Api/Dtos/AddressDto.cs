namespace Snacks.Api.Dtos;

public record AddressDto
{
    public required int AddressId { get; set; }
    public required int Zipcode { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required int StreetNumber { get; set; }
    public required decimal GpsLongitude { get; set; }
    public required decimal GpsLatitude { get; set; }
    public string? FreeText { get; set; }
}
