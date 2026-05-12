using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.MenuItems.Commands.UpdateMenuItem;

public sealed record UpdateMenuItemCommand(
    Guid Id,
    string Name,
    decimal Price,
    bool IsAvailable,
    Guid RestaurantId) : ICommand;