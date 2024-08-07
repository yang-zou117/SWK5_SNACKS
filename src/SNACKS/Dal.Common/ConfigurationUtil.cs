namespace Dal.Common;

using Microsoft.Extensions.Configuration;

public static class ConfigurationUtil
{
  private static IConfiguration? _configuration;

  public static IConfiguration GetConfiguration() =>
    _configuration ??= new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false)
      .Build();

  public static (string ConnectionString, string ProviderName) GetConnectionParameters(string configName)
  {
    return GetConnectionParameters(GetConfiguration(), configName);
  }

  public static (string ConnectionString, string ProviderName) GetConnectionParameters(IConfiguration configuration, string configName)
  {
    var connectionConfig = configuration.GetSection("ConnectionStrings").GetSection(configName);
    if (!connectionConfig.Exists())
    {
      throw new ArgumentException($"Connection configuration '{configName}' does not exist");
    }

    var connectionString = connectionConfig["ConnectionString"];
    if (connectionString is null)
    {
      throw new ArgumentException($"Property ConnectionString not defined in configuration '{configName}'.");
    }

    var providerName = connectionConfig["ProviderName"];
    if (providerName is null)
    {
      throw new ArgumentException($"Property ProviderName not defined in configuration '{configName}'.");
    }

    return (connectionString, providerName);
  }
}
