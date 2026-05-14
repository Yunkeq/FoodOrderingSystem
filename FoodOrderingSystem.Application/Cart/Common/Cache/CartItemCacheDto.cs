namespace FoodOrderingSystem.Application.Cart.Common.Cache;

public sealed record CartItemCacheDto(
    Guid MenuItemId,
    int Quantity);
