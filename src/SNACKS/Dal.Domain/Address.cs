namespace Dal.Domain;

public class Address
{
    public Address(int addressId, int zipcode, 
                   string city, string street, int streetNumber,
                   decimal gpsLongitude, decimal gpsLatitude, string? freeText)
    {
        AddressId = addressId;
        Zipcode = zipcode;
        City = city ?? throw new ArgumentNullException(nameof(city));
        Street = street ?? throw new ArgumentNullException(nameof(street));
        StreetNumber = streetNumber;
        GpsLongitude = gpsLongitude;
        GpsLatitude = gpsLatitude;
        FreeText = freeText;
    }

    public Address()
    {

    }
  
    public int AddressId { get; set; }
    public int Zipcode { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public int StreetNumber { get; set; }
    public decimal GpsLongitude { get; set; }
    public decimal GpsLatitude { get; set; }
    public string? FreeText { get; set; }

    public override string ToString() =>
        $"Address(AddressId: {AddressId}, " +
        $"Zipcode: {Zipcode}, " +
        $"City: {City}, " +
        $"Street: {Street}, " +
        $"StreetNumber: {StreetNumber}, " +
        $"GpsLongitude: {GpsLongitude}, " +
        $"GpsLatitude: {GpsLatitude})" +
        $"FreeText: {FreeText})";

    public override bool Equals(object obj) =>
        obj is Address address &&
        AddressId == address.AddressId &&
        Zipcode == address.Zipcode &&
        City == address.City &&
        Street == address.Street &&
        StreetNumber == address.StreetNumber &&
        GpsLongitude == address.GpsLongitude &&
        GpsLatitude == address.GpsLatitude &&
        FreeText == address.FreeText;

}