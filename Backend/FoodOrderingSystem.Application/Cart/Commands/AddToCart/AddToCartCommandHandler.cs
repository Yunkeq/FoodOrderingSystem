using FoodOrderingSystem.Application.Abstractions.Caching;
using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Cart.Common.Cache;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Cart.Commands.AddToCart;

public sealed class AddToCartCommandHandler : ICommandHandler<AddToCartCommand>
{
    private readonly ICartService _cartService;
    private readonly IApplicationDbContext _dbContext;

    public AddToCartCommandHandler(ICartService cartService, IApplicationDbContext dbContext)
    {
        _cartService = cartService;
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(AddToCartCommand command, CancellationToken cancellationToken)
    {
        var menuItem = await _dbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(mi => mi.Id == command.MenuItemId, cancellationToken);

        if (menuItem == null)
        {
            return Result.Failure(MenuItemErrors.MenuItemNotFound(command.MenuItemId));
        }

        if (!menuItem.IsAvailable)
        {
            return Result.Failure(MenuItemErrors.MenuItemIsNotAvailable(command.MenuItemId));
        }

        var cart = await _cartService.GetCartAsync(command.UserId, cancellationToken);

        var items = cart.MenuItems;

        var existingItem = items.FirstOrDefault(i => i.MenuItemId == command.MenuItemId);

        if (existingItem != null)
        {
            items.Remove(existingItem);

            items.Add(existingItem with
            {
                Quantity = existingItem.Quantity + command.Quantity,
            });
        }
        else
        {
            items.Add(new CartItemCacheDto(
                MenuItemId: command.MenuItemId,
                Quantity: command.Quantity));
        }

        var updatedCart = new CartCacheDto(
            UserId: cart.UserId,
            MenuItems: items);

        await _cartService.SetCartAsync(updatedCart, cancellationToken);

        return Result.Success();
    }
}
