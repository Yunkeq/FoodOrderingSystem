using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.Cart.Commands.AddToCart;

public sealed record AddToCartCommand(Guid UserId, Guid MenuItemId, int Quantity) : ICommand;
