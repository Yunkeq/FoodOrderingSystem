using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Restaurants.Commands.UpdateRestaurant;

public sealed class UpdateRestaurantCommandHandler : ICommandHandler<UpdateRestaurantCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateRestaurantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

        if (restaurant is null)
        {
            return Result.Failure(RestaurantErrors.RestaurantNotFound(command.Id));
        }

        restaurant.Name = command.Name;
        restaurant.City = command.City;
        restaurant.IsOpen = command.IsOpen;

        return Result.Success();
    }
}
