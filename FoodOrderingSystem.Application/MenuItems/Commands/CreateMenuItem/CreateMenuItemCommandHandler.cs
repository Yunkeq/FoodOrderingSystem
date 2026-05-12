using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Domain.Entities;

namespace FoodOrderingSystem.Application.MenuItems.Commands.CreateMenuItem;

public sealed class CreateMenuItemCommandHandler : ICommandHandler<CreateMenuItemCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateMenuItemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(CreateMenuItemCommand command, CancellationToken cancellationToken)
    {
        var menuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Price = command.Price,
            IsAvailable = command.IsAvailable,
            RestaurantId = command.RestaurantId,
        };

        await _dbContext.MenuItems.AddAsync(menuItem, cancellationToken);

        return Result<Guid>.Success(menuItem.Id);
    }
}