using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Restaurants.Common;

namespace FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurantById;

public sealed record GetRestaurantByIdQuery(Guid Id) : IQuery<RestaurantDto>;
