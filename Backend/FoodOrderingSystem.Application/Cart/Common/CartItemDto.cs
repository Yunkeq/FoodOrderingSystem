namespace FoodOrderingSystem.Application.Cart.Common;

public sealed record class CartItemDto(
    Guid ItemId,
    string ItemName,
    decimal Price,
    int Quantity);
