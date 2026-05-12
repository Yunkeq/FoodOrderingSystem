namespace FoodOrderingSystem.Api.Contracts.MenuItems;

public sealed record MenuItemResponse(
    Guid Id,
    string Name,
    decimal Price,
    bool IsAvailable,
    Guid RestaurantId);