namespace FoodOrderingSystem.Application.Orders.Common;

public sealed record OrderItemDto(
    Guid MenuItemId,
    string Name,
    decimal Price,
    int Quantity);