namespace FoodOrderingSystem.Application.Cart.Common;

public sealed record CartDto(
    Guid UserId,
    List<CartItemDto> Items,
    decimal TotalPrice);
