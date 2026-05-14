using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.Cart.Commands.ClearCart;

public sealed record ClearCartCommand(Guid UserId) : ICommand;