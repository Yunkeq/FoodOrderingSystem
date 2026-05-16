using FoodOrderingSystem.Application.Abstractions.Caching;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Cart.Common.Cache;
using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Cart.Commands.DeleteCartItem;

public sealed class DeleteCartItemCommandHandler : ICommandHandler<DeleteCartItemCommand>
{
    private readonly ICartService _cartService;

    public DeleteCartItemCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<Result> Handle(DeleteCartItemCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetCartAsync(command.UserId, cancellationToken);

        if (cart.MenuItems.Count == 0)
        {
            return Result.Success();
        }

        var existingItem = cart.MenuItems.FirstOrDefault(i => i.MenuItemId == command.MenuItemId);

        // item not in cart - success.
        if (existingItem is null)
        {
            return Result.Success();
        }

        var newQuantity = existingItem.Quantity - command.Quantity;

        if (newQuantity > 0)
        {
            cart.MenuItems.Remove(existingItem);

            cart.MenuItems.Add(existingItem with
            {
                Quantity = newQuantity,
            });

            var updatedCart = new CartCacheDto(
                UserId: cart.UserId,
                MenuItems: cart.MenuItems);

            await _cartService.SetCartAsync(updatedCart, cancellationToken);

            return Result.Success();
        }

        cart.MenuItems.Remove(existingItem);

        if (cart.MenuItems.Count == 0)
        {
            await _cartService.RemoveCartAsync(command.UserId, cancellationToken);
            return Result.Success();
        }

        var cartAfterRemove = new CartCacheDto(
            UserId: cart.UserId,
            MenuItems: cart.MenuItems);

        await _cartService.SetCartAsync(cartAfterRemove, cancellationToken);

        return Result.Success();
    }
}