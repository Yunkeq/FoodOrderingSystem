namespace FoodOrderingSystem.Api.Contracts.Restaurants;

public sealed record RestaurantResponse(
    Guid Id,
    string Name,
    string City,
    bool IsOpen);
