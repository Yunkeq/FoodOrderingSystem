using FoodOrderingSystem.Application.Abstractions.Caching;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Cart.Commands.ClearCart;

public sealed class ClearCartCommandHandler : ICommandHandler<ClearCartCommand>
{
    private readonly ICartService _cartService;

    public ClearCartCommandHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<Result> Handle(ClearCartCommand command, CancellationToken cancellationToken)
    {
        await _cartService.RemoveCartAsync(command.UserId, cancellationToken);
        return Result.Success();
    }
}