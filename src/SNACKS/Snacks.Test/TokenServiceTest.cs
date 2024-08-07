
namespace Snacks.Test;
public class TokenServiceTest
{
    [Fact]
    public void EncodeToken_ShouldEncode()
    {
        var parameters = new Dictionary<string, string>
        {
            { "userId", "1" },
            { "username", "test" },
            { "role", "admin" }
        };

        var token = TokenService.EncodeToken(parameters);
        Assert.NotNull(token);
    }

    [Fact]
    public void DecodeToken_ShouldDecode()
    {
        var parameters = new Dictionary<string, string>
        {
            { "userId", "1" },
            { "username", "test" },
            { "role", "admin" }
        };

        var token = TokenService.EncodeToken(parameters);
        var decodedParameters = TokenService.DecodeToken(token);
        
        Assert.Contains(decodedParameters, x => x.Key == "userId" && x.Value == "1");
        Assert.Contains(decodedParameters, x => x.Key == "username" && x.Value == "test");
        Assert.Contains(decodedParameters, x => x.Key == "role" && x.Value == "admin");
    }

    [Fact]
    public void DecodeToken_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => TokenService.DecodeToken("invalid token"));
    }
}
