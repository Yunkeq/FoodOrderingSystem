using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.Restaurants.Commands.UpdateRestaurant;

public sealed record UpdateRestaurantCommand(
    Guid Id,
    string Name,
    string City,
    bool IsOpen) : ICommand;
