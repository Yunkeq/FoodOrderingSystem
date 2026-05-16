using FoodOrderingSystem.Application.Cart.Common.Cache;

namespace FoodOrderingSystem.Application.Abstractions.Caching;

public interface ICartService
{
    Task<CartCacheDto> GetCartAsync(Guid userId, CancellationToken cancellationToken);
    Task SetCartAsync(CartCacheDto cart, CancellationToken cancellationToken);
    Task RemoveCartAsync(Guid userId, CancellationToken cancellationToken);
}
