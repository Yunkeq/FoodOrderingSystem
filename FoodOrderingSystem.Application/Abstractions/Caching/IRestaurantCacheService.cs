using FoodOrderingSystem.Application.Restaurants.Common;

namespace FoodOrderingSystem.Application.Abstractions.Services;

public interface IRestaurantCacheService
{
    Task<IReadOnlyCollection<RestaurantDto>> GetAllRestaurants(CancellationToken cancellationToken);
    Task SetAllRestaurants(IReadOnlyCollection<RestaurantDto> restaurants, CancellationToken cancellationToken);
}
