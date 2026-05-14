using Dapper;
using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.Restaurants.Common;

namespace FoodOrderingSystem.Infrastructure.Persistance.Repositories;

public sealed class RestaurantRepository : IRestaurantRepository
{
    private const string DbSchema = "FoodOrdering";
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RestaurantRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IReadOnlyCollection<RestaurantDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = $"""
            SELECT "Id", "Name", "City", "IsOpen"
            FROM "{DbSchema}"."Restaurants";
            """;

        var restaurants = await connection.QueryAsync<RestaurantDto>(
            sql,
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return restaurants.ToList();
    }

    public async Task<RestaurantDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = $"""
            SELECT "Id", "Name", "City", "IsOpen"
            FROM "{DbSchema}"."Restaurants"
            WHERE "Id" = @Id;
            """;

        return await connection.QuerySingleOrDefaultAsync<RestaurantDto>(
            new CommandDefinition(sql, parameters: new { Id = id }, cancellationToken: cancellationToken));
    }
}