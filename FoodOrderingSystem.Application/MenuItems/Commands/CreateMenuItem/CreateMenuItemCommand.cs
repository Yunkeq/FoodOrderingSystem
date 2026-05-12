using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.MenuItems.Commands.CreateMenuItem;

public sealed record CreateMenuItemCommand(
    string Name,
    decimal Price,
    bool IsAvailable,
    Guid RestaurantId) : ICommand<Guid>;