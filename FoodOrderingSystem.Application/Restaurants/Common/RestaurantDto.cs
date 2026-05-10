namespace FoodOrderingSystem.Application.Restaurants.Common;

public sealed record RestaurantDto(
    Guid Id,
    string Name,
    string City,
    bool IsOpen);