using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.Orders.Commands.PlaceOrder;

public sealed record PlaceOrderCommand(Guid UserId) : ICommand<Guid>;