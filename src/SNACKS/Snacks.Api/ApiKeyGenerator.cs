

namespace Snacks.Api;

public static class ApiKeyGenerator
{
    public static string GenerateApiKeyValue()
    {
        const int length = 38;
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string result = "";
        
        for (int i = 0; i < length; i++)
        {
            result += chars[new Random().Next(chars.Length)];
        }

        return result;
    }

}
