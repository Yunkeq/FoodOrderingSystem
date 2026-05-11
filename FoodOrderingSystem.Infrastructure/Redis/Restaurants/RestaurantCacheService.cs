using System.Text.Json;
using FoodOrderingSystem.Application.Abstractions.Services;
using FoodOrderingSystem.Application.Restaurants.Common;
using Microsoft.Extensions.Caching.Distributed;

namespace FoodOrderingSystem.Infrastructure.Redis.Restaurants;

public sealed class RestaurantCacheService : IRestaurantCacheService
{
    private const int CacheDurationInMinutes = 10;
    private readonly IDistributedCache _distributedCache;

    public RestaurantCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<IReadOnlyCollection<RestaurantDto>> GetAllRestaurants(CancellationToken cancellationToken)
    {
        var restaurants = await _distributedCache
            .GetStringAsync(GetCacheKeyForAllRestaurants(), cancellationToken);

        if (string.IsNullOrEmpty(restaurants))
        {
            return new List<RestaurantDto>();
        }

        return JsonSerializer.Deserialize<IReadOnlyCollection<RestaurantDto>>(restaurants)
            ?? throw new JsonException("Cached restaurants payload deserialized to null.");
    }

    public async Task SetAllRestaurants(IReadOnlyCollection<RestaurantDto> restaurants, CancellationToken cancellationToken)
    {
        await _distributedCache.SetStringAsync(
            GetCacheKeyForAllRestaurants(),
            JsonSerializer.Serialize(restaurants),
            new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheDurationInMinutes) },
            cancellationToken);
    }

    private string GetCacheKeyForAllRestaurants() => "restaurants:all";
}
