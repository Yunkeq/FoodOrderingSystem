using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.Restaurants.Commands.DeleteRestaurant;

public sealed record DeleteRestaurantCommand(Guid Id) : ICommand;
