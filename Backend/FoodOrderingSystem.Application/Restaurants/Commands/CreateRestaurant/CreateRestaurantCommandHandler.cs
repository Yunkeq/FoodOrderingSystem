using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Domain.Entities;

namespace FoodOrderingSystem.Application.Restaurants.Commands.CreateRestaurant;

public sealed class CreateRestaurantCommandHandler : ICommandHandler<CreateRestaurantCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateRestaurantCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
    {
        var restaurant = new Restaurant
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            City = command.City,
            IsOpen = command.IsOpen,
        };

        await _dbContext.Restaurants.AddAsync(restaurant);

        return Result<Guid>.Success(restaurant.Id);
    }
}
