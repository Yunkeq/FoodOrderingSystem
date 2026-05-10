using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Restaurants.Common;

namespace FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurantById;

public sealed class GetRestaurantByIdQueryHandler : IQueryHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetRestaurantByIdQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<Result<RestaurantDto>> Handle(GetRestaurantByIdQuery query, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(query.Id, cancellationToken);

        if (restaurant is null)
        {
            return Result<RestaurantDto>.Failure(RestaurantErrors.RestaurantNotFound(query.Id));
        }

        return Result<RestaurantDto>.Success(restaurant);
    }
}
