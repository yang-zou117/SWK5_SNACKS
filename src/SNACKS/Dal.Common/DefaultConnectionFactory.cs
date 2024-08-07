namespace Dal.Common;

using System.Data.Common;
using Microsoft.Extensions.Configuration;

public class DefaultConnectionFactory : IConnectionFactory
{
    private readonly DbProviderFactory dbProviderFactory;

    public static IConnectionFactory FromConfiguration(IConfiguration configuration, string connectionStringConfigName)
    {
        (string connectionString, string providerName) =
          ConfigurationUtil.GetConnectionParameters(configuration, connectionStringConfigName);
        return new DefaultConnectionFactory(connectionString, providerName);
    }

    private DefaultConnectionFactory(string connectionString, string providerName)
    {
        this.ConnectionString = connectionString;
        this.ProviderName = providerName;

        DbUtil.RegisterAdoProviders();
        this.dbProviderFactory = DbProviderFactories.GetFactory(providerName);
    }

    public string ConnectionString { get; }

    public string ProviderName { get; }

    public async Task<DbConnection> CreateConnection()
    {
        DbConnection? connection = dbProviderFactory.CreateConnection();
        if (connection is null)
            throw new InvalidOperationException("DbProviderFactoryGetConnection ==> null");

        connection.ConnectionString = ConnectionString;
        await connection.OpenAsync();
        
        return connection; 
    }
}
