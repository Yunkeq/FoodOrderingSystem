namespace FoodOrderingSystem.Application.MenuItems.Common;

public sealed record MenuItemDto(
    Guid Id,
    string Name,
    decimal Price,
    bool IsAvailable,
    Guid RestaurantId);