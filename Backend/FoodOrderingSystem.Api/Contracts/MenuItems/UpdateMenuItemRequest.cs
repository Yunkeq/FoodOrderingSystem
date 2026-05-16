namespace FoodOrderingSystem.Api.Contracts.MenuItems;

public sealed record UpdateMenuItemRequest(
    string Name,
    decimal Price,
    bool IsAvailable,
    Guid RestaurantId);