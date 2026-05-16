using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.Abstractions.Services;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Restaurants.Common;

namespace FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurants;

public sealed class GetRestaurantsQueryHandler : IQueryHandler<GetRestaurantsQuery, IReadOnlyCollection<RestaurantDto>>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IRestaurantCacheService _cacheService;

    public GetRestaurantsQueryHandler(
        IRestaurantRepository restaurantRepository,
        IRestaurantCacheService cacheService)
    {
        _restaurantRepository = restaurantRepository;
        _cacheService = cacheService;
    }

    public async Task<Result<IReadOnlyCollection<RestaurantDto>>> Handle(GetRestaurantsQuery query, CancellationToken cancellationToken)
    {
        var restaurants = await _cacheService.GetAllRestaurants(cancellationToken);

        if (restaurants.Count == 0)
        {
            restaurants = await _restaurantRepository.GetAllAsync(cancellationToken);

            if (restaurants.Count == 0)
            {
                return Result<IReadOnlyCollection<RestaurantDto>>.Success(restaurants);
            }

            await _cacheService.SetAllRestaurants(restaurants, cancellationToken);
        }

        return Result<IReadOnlyCollection<RestaurantDto>>.Success(restaurants);
    }
}
