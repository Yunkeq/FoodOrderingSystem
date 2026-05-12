using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.MenuItems.Commands.UpdateMenuItem;

public sealed class UpdateMenuItemCommandHandler : ICommandHandler<UpdateMenuItemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateMenuItemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateMenuItemCommand command, CancellationToken cancellationToken)
    {
        var menuItem = await _dbContext.MenuItems
            .FirstOrDefaultAsync(mi => mi.Id == command.Id, cancellationToken);

        if (menuItem is null)
        {
            return Result.Failure(MenuItemErrors.MenuItemNotFound(command.Id));
        }

        var restaurantExists = await _dbContext.Restaurants
            .AnyAsync(r => r.Id == command.RestaurantId, cancellationToken);

        if (!restaurantExists)
        {
            return Result.Failure(RestaurantErrors.RestaurantNotFound(command.RestaurantId));
        }

        menuItem.Name = command.Name;
        menuItem.Price = command.Price;
        menuItem.IsAvailable = command.IsAvailable;
        menuItem.RestaurantId = command.RestaurantId;

        return Result.Success();
    }
}