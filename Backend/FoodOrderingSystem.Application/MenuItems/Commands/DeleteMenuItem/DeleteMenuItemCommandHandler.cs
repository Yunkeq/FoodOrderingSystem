using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.MenuItems.Commands.DeleteMenuItem;

public sealed class DeleteMenuItemCommandHandler : ICommandHandler<DeleteMenuItemCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteMenuItemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteMenuItemCommand command, CancellationToken cancellationToken)
    {
        var deleted = await _dbContext.MenuItems
            .Where(mi => mi.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleted == 0)
        {
            return Result.Failure(MenuItemErrors.MenuItemNotFound(command.Id));
        }

        return Result.Success();
    }
}