using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Restaurants.Common;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurants;

public sealed class GetRestaurantsQueryHandler : IQueryHandler<GetRestaurantsQuery, IReadOnlyCollection<RestaurantDto>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<Result<IReadOnlyCollection<RestaurantDto>>> Handle(GetRestaurantsQuery query, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetAllAsync(cancellationToken);

        return Result<IReadOnlyCollection<RestaurantDto>>.Success(restaurants);
    }
}
