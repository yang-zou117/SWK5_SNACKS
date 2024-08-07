using Dal.Common;
using Dal.Domain;
using Dal.Interface;
using System.Data;

namespace Dal.Dao;

public class AddressDao : IAddressDao
{
    private readonly AdoTemplate template;

    public AddressDao(IConnectionFactory connectionFactory)
    {
        template = new AdoTemplate(connectionFactory);
    }

    private Address MapRowToAddress(IDataRecord row)
    {
        string? freeText = row["free_text"] == DBNull.Value ? null : (string)row["free_text"];

        return new Address(
               (int)row["address_id"],
               (int)row["zipcode"],
               (string)row["city"],
               (string)row["street"],
               (int)row["street_number"],
               (decimal)row["gps_longitude"],
               (decimal)row["gps_latitude"],
               freeText
               );
    }

    public async Task<int> InsertAddressAsync(Address newAddress)
    {
        const string sql = "INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude) " +
                           "VALUES (@zipcode, @city, @street, @street_number, @gps_longitude, @gps_latitude);" +
                           "SELECT LAST_INSERT_ID();";

        return await
            template.ExecuteScalarAsync<int>(sql, new QueryParameter("@zipcode", newAddress.Zipcode),
                                                  new QueryParameter("@city", newAddress.City),
                                                  new QueryParameter("@street", newAddress.Street),
                                                  new QueryParameter("@street_number", newAddress.StreetNumber),
                                                  new QueryParameter("@gps_longitude", newAddress.GpsLongitude),
                                                  new QueryParameter("@gps_latitude", newAddress.GpsLatitude));
    }

    public async Task<Address?> GetAddressByIdAsync(int addressId)
    {
        const string sql = "SELECT * FROM address WHERE address_id = @address_id";

        return await template.QuerySingleAsync(sql, MapRowToAddress, new QueryParameter("@address_id", addressId));
    }

    public async Task<bool> DeleteAddressAsync(int addressId)
    {
        return await template.ExecuteAsync("DELETE FROM address WHERE address_id = @address_id", new QueryParameter("@address_id", addressId)) == 1;
    }

    public Task<int> GetDistanceToDeliveryAddressAsync(int restaurantId, Address deliveryAddress)
    {
        const string sql = "SELECT ROUND( " +
                                        "(ST_Distance_Sphere(POINT(@delivery_longitude, @delivery_latitude), " +
                                                            "POINT(address.gps_longitude, address.gps_latitude))) / 1000" +
                                       ") AS distance " +
                           "FROM address " +
                           "INNER JOIN restaurant ON address.address_id = restaurant.address_id " +
                           "WHERE restaurant.restaurant_id = @restaurant_id";

        return template.ExecuteScalarAsync<int>(sql, new QueryParameter("@restaurant_id", restaurantId),
                                                     new QueryParameter("@delivery_longitude", deliveryAddress.GpsLongitude),
                                                     new QueryParameter("@delivery_latitude", deliveryAddress.GpsLatitude));
    }

    public async Task<Address?> GetAddressForRestaurantAsync(int restaurantId)
    {

        const string sql = "SELECT address.address_id, address.zipcode, address.city, address.street, " +
                           "address.street_number, address.gps_longitude, address.gps_latitude, address.free_text " +
                           "FROM address " +
                            "INNER JOIN restaurant ON address.address_id = restaurant.address_id " +
                           "WHERE restaurant.restaurant_id = @restaurant_id";

        return await template.QuerySingleAsync(sql, MapRowToAddress, 
                                               new QueryParameter("@restaurant_id", restaurantId));
    }
}
