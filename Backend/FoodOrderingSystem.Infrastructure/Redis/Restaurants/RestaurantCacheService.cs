using System.Text.Json;
using FoodOrderingSystem.Application.Abstractions.Services;
using FoodOrderingSystem.Application.Restaurants.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FoodOrderingSystem.Infrastructure.Redis.Restaurants;

public sealed class RestaurantCacheService : IRestaurantCacheService
{
    private const int CacheDurationInMinutes = 10;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<RestaurantCacheService> _logger;

    public RestaurantCacheService(
        IDistributedCache distributedCache,
        ILogger<RestaurantCacheService> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<RestaurantDto>> GetAllRestaurants(CancellationToken cancellationToken)
    {
        var restaurants = await _distributedCache
            .GetStringAsync(GetCacheKeyForAllRestaurants(), cancellationToken);

        if (string.IsNullOrEmpty(restaurants))
        {
            return new List<RestaurantDto>();
        }

        try
        {
            return JsonSerializer.Deserialize<IReadOnlyCollection<RestaurantDto>>(restaurants)
                ?? new List<RestaurantDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize cached restaurants payload. Cache key: {CacheKey}", GetCacheKeyForAllRestaurants());
            return new List<RestaurantDto>();
        }
    }

    public async Task SetAllRestaurants(IReadOnlyCollection<RestaurantDto> restaurants, CancellationToken cancellationToken)
    {
        await _distributedCache.SetStringAsync(
            GetCacheKeyForAllRestaurants(),
            JsonSerializer.Serialize(restaurants),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheDurationInMinutes),
            },
            cancellationToken);
    }

    private string GetCacheKeyForAllRestaurants() => "restaurants:all";
}
