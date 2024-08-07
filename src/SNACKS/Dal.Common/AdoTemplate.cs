using Dal.Common;
using System.Data;
using System.Data.Common;

namespace Dal.Common;

public delegate T RowMapper<T>(IDataRecord row);

public class AdoTemplate
{
    private readonly IConnectionFactory connectionFactory;

    public AdoTemplate(IConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }

    private void AddParameters(DbCommand command, QueryParameter[] parameters)
    {
        foreach (var p in parameters)
        {
            var dbParam = command.CreateParameter();
            dbParam.ParameterName = p.Name;
            dbParam.Value = p.Value;
            command.Parameters.Add(dbParam);
        }
    }

    // method for executing a query and return a list of T objects
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, RowMapper<T> rowMapper, 
                                                    params QueryParameter[] parameters)
    {
        using DbConnection connection = await connectionFactory.CreateConnection();

        using DbCommand command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameters(command, parameters);

        using DbDataReader reader = await command.ExecuteReaderAsync();

        IList<T> resultList = new List<T>();
        while (await reader.ReadAsync())
        {
            resultList.Add(rowMapper(reader));
        }
        return resultList;
    }

    // method for executing a query and return a single T object
    public async Task<T?> QuerySingleAsync<T>(string sql, RowMapper<T> rowMapper, 
                                              params QueryParameter[] parameters)
    {
        return (await QueryAsync(sql, rowMapper, parameters)).SingleOrDefault();
    }

    // method for DML statements
    public async Task<int> ExecuteAsync(string sql, params QueryParameter[] parameters)
    {
        using DbConnection connection = await connectionFactory.CreateConnection();

        using DbCommand command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameters(command, parameters);

        return await command.ExecuteNonQueryAsync();
    }

    // method for insert statements, returns the id of inserted row
    public async Task<R> ExecuteScalarAsync<R>(string sql, params QueryParameter[] parameters)
    {
        using DbConnection connection = await connectionFactory.CreateConnection();

        using DbCommand command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameters(command, parameters);

        object result = await command.ExecuteScalarAsync() ?? 
            throw new ArgumentException("Empty result in executeScalaraAsync");

        return (R)Convert.ChangeType(result, typeof(R));
    }



}
