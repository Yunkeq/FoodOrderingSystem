using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Restaurants.Commands.DeleteRestaurant;

public sealed class DeleteRestaurantCommandHandler : ICommandHandler<DeleteRestaurantCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteRestaurantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteRestaurantCommand command, CancellationToken cancellationToken)
    {
        var deleted = await _dbContext.Restaurants
            .Where(r => r.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleted == 0)
        {
            return Result.Failure(RestaurantErrors.RestaurantNotFound(command.Id));
        }

        return Result.Success();
    }
}
