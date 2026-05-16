using FoodOrderingSystem.Application.Abstractions.Caching;
using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Orders.Commands.PlaceOrder;

public sealed class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, Guid>
{
    private readonly ICartService _cartService;
    private readonly IApplicationDbContext _dbContext;

    public PlaceOrderCommandHandler(
        ICartService cartService,
        IApplicationDbContext dbContext)
    {
        _cartService = cartService;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetCartAsync(command.UserId, cancellationToken);

        if (cart.MenuItems.Count == 0)
        {
            return Result<Guid>.Failure(OrderErrors.CartIsEmpty());
        }

        var menuItemIds = cart.MenuItems
            .Select(x => x.MenuItemId)
            .Distinct()
            .ToList();

        var menuItems = await _dbContext.MenuItems
            .AsNoTracking()
            .Where(mi => menuItemIds.Contains(mi.Id))
            .Select(mi => new
            {
                mi.Id,
                mi.Name,
                mi.Price,
                mi.IsAvailable,
            })
            .ToListAsync(cancellationToken);

        var menuItemsById = menuItems.ToDictionary(x => x.Id);

        foreach (var cartItem in cart.MenuItems)
        {
            if (!menuItemsById.TryGetValue(cartItem.MenuItemId, out var mi))
            {
                return Result<Guid>.Failure(MenuItemErrors.MenuItemNotFound(cartItem.MenuItemId));
            }

            if (!mi.IsAvailable)
            {
                return Result<Guid>.Failure(MenuItemErrors.MenuItemIsNotAvailable(cartItem.MenuItemId));
            }
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = command.UserId,
            OrderDate = DateTime.UtcNow,
        };

        foreach (var cartItem in cart.MenuItems)
        {
            var mi = menuItemsById[cartItem.MenuItemId];

            order.OrderItems.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                MenuItemId = mi.Id,
                Name = mi.Name,
                Price = mi.Price,
                Quantity = cartItem.Quantity,
            });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.Quantity);
        order.TotalPrice = order.OrderItems.Sum(i => i.Price * i.Quantity);

        await _dbContext.Orders.AddAsync(order, cancellationToken);

        await _cartService.RemoveCartAsync(command.UserId, cancellationToken);

        return Result<Guid>.Success(order.Id);
    }
}