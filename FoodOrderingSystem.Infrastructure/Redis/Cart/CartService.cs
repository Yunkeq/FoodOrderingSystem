using System.Text.Json;
using FoodOrderingSystem.Application.Abstractions.Caching;
using FoodOrderingSystem.Application.Cart.Common;
using FoodOrderingSystem.Application.Cart.Common.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FoodOrderingSystem.Infrastructure.Redis.Cart;

public sealed class CartService : ICartService
{
    private const int CartExpirationHours = 2;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CartService> _logger;

    public CartService(
        IDistributedCache distributedCache,
        ILogger<CartService> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<CartCacheDto> GetCartAsync(Guid userId, CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(userId);
        var cartJson = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

        if (cartJson == null)
        {
            return CreateEmptyCart(userId);
        }

        try
        {
            return JsonSerializer.Deserialize<CartCacheDto>(cartJson) ?? CreateEmptyCart(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize cart JSON. Cache key: {CacheKey}", cacheKey);
            return CreateEmptyCart(userId);
        }
    }

    public async Task RemoveCartAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(GetCacheKey(userId), cancellationToken);
    }

    public async Task SetCartAsync(CartCacheDto cart, CancellationToken cancellationToken)
    {
        await _distributedCache.SetStringAsync(
            GetCacheKey(cart.UserId),
            JsonSerializer.Serialize(cart),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CartExpirationHours),
            },
            cancellationToken);
    }

    private static string GetCacheKey(Guid userId) => $"cart:{userId}";

    private static CartCacheDto CreateEmptyCart(Guid userId)
    {
        return new CartCacheDto(
            UserId: userId,
            MenuItems: new List<CartItemCacheDto>());
    }
}
