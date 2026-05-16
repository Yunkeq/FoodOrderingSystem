using FoodOrderingSystem.Application.Abstractions.Caching;
using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Cart.Common;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Cart.Queries.GetCart;

public sealed class GetCartQueryHandler : IQueryHandler<GetCartQuery, CartDto>
{
    private readonly ICartService _cartService;
    private readonly IApplicationDbContext _dbContext;

    public GetCartQueryHandler(
        ICartService cartService,
        IApplicationDbContext dbContext)
    {
        _cartService = cartService;
        _dbContext = dbContext;
    }

    public async Task<Result<CartDto>> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        var cachedCart = await _cartService.GetCartAsync(query.UserId, cancellationToken);

        if (cachedCart.MenuItems.Count == 0)
        {
            return Result<CartDto>.Success(
                new CartDto(
                    UserId: cachedCart.UserId,
                    Items: [],
                    TotalPrice: 0m));
        }

        var itemIds = cachedCart.MenuItems
            .Select(i => i.MenuItemId)
            .Distinct()
            .ToList();

        var menuItemsById = await _dbContext.MenuItems
            .Where(mi => itemIds.Contains(mi.Id))
            .Select(mi => new
            {
                mi.Id,
                mi.Name,
                mi.Price,
            })
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var items = cachedCart.MenuItems
            .Where(i => menuItemsById.ContainsKey(i.MenuItemId))
            .Select(i =>
            {
                var mi = menuItemsById[i.MenuItemId];

                return new CartItemDto(
                    ItemId: mi.Id,
                    ItemName: mi.Name,
                    Price: mi.Price,
                    Quantity: i.Quantity);
            })
            .ToList();

        var totalPrice = items.Sum(i => i.Price * i.Quantity);

        var cart = new CartDto(
            UserId: cachedCart.UserId,
            Items: items,
            TotalPrice: totalPrice);

        return Result<CartDto>.Success(cart);
    }
}
