using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Restaurants.Common;

namespace FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurants;

public sealed record GetRestaurantsQuery() : IQuery<IReadOnlyCollection<RestaurantDto>>;
