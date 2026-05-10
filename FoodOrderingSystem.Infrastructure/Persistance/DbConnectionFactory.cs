using System.Data;
using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Common.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace FoodOrderingSystem.Infrastructure.Persistance;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IOptions<DbOptions> _dbOptions;

    public DbConnectionFactory(IOptions<DbOptions> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public IDbConnection CreateConnection()
    {
        var builder = new NpgsqlConnectionStringBuilder(_dbOptions.Value.ConnectionString)
        {
            SearchPath = _dbOptions.Value.Schema,
        };

        return new NpgsqlConnection(builder.ConnectionString);
    }
}
