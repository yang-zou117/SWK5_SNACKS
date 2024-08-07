using Dal.Common;
using Dal.Dao;
using Dal.Domain;
using Dal.Interface;

namespace Snacks.Test;

public class AddressDaoTest
{
    private readonly IAddressDao addressDao;

    public AddressDaoTest()
    {
        var configuration = ConfigurationUtil.GetConfiguration();
        var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "SnackDbConnection");
        addressDao = new AddressDao(connectionFactory);
    }

    [Fact]
    public async Task InsertAddressAsync_ShouldInsert()
    {
        var newAddress = new Address(0, 1234, "Test City", "Test Street", 1,
                                      12.23249348392089M, 33.25641564564145M, null);
        var addressId = await addressDao.InsertAddressAsync(newAddress);
        Assert.True(addressId > 0);
    }

    [Fact]
    public async Task GetAddressAsync_ShouldGetAddress()
    {
        var newAddress = new Address(0, 1234, "Test City", "Test Street", 1,
                                      12.23249348392089M, 33.25641564564145M, null);
        var addressId = await addressDao.InsertAddressAsync(newAddress);
        newAddress.AddressId = addressId;
        Assert.Equal(newAddress, await addressDao.GetAddressByIdAsync(addressId));
    }

    [Fact]
    public async Task DeleteAddressAsync_ShouldDelete()
    {
        var newAddress = new Address(0, 1234, "Test City", "Test Street", 1,
                                      12.23249348392089M, 33.25641564564145M, null);
        var addressId = await addressDao.InsertAddressAsync(newAddress);
        Assert.True(await addressDao.DeleteAddressAsync(addressId));
    }
}
