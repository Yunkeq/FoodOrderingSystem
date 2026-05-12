namespace FoodOrderingSystem.Api.Contracts.MenuItems;

public sealed record CreateMenuItemRequest(
    string Name,
    decimal Price,
    bool IsAvailable,
    Guid RestaurantId);