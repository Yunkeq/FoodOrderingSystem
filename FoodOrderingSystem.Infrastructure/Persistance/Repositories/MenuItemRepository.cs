using Dapper;
using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.MenuItems.Common;

namespace FoodOrderingSystem.Infrastructure.Persistance.Repositories;

public sealed class MenuItemRepository : IMenuItemRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MenuItemRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IReadOnlyCollection<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT "Id", "Name", "Price", "IsAvailable", "RestaurantId"
            FROM "FoodOrdering"."MenuItems";
            """;

        var items = await connection.QueryAsync<MenuItemDto>(
            sql,
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return items.ToList();
    }

    public async Task<IReadOnlyCollection<MenuItemDto>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql =
            """
            SELECT "Id", "Name", "Price", "IsAvailable", "RestaurantId"
            FROM "FoodOrdering"."MenuItems"
            WHERE "RestaurantId" = @RestaurantId;
            """;

        var items = await connection.QueryAsync<MenuItemDto>(
            new CommandDefinition(sql, parameters: new { RestaurantId = restaurantId }, cancellationToken: cancellationToken));

        return items.ToList();
    }
}