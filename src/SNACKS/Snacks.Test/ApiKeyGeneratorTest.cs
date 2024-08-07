

using Snacks.Api;

namespace Snacks.Test;
public class ApiKeyGeneratorTest
{
    [Fact]
    public void GenerateApiKey_ShouldGenerate()
    {
        var apiKey = ApiKeyGenerator.GenerateApiKeyValue();
        Assert.NotNull(apiKey);
        Assert.True(apiKey.Length > 0);
    }
}
