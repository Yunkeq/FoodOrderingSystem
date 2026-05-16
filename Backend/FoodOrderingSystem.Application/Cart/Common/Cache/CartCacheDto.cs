namespace FoodOrderingSystem.Application.Cart.Common.Cache;

public sealed record CartCacheDto(Guid UserId, List<CartItemCacheDto> MenuItems);
