using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.Restaurants.Commands.CreateRestaurant;

public sealed record CreateRestaurantCommand(
    string Name,
    string City,
    bool IsOpen) : ICommand<Guid>;
