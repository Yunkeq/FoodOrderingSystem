using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.Cart.Commands.DeleteCartItem;

public sealed record DeleteCartItemCommand(Guid UserId, Guid MenuItemId, int Quantity) : ICommand;