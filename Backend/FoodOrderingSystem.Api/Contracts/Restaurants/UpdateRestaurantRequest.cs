namespace FoodOrderingSystem.Api.Contracts.Restaurants;

public sealed record UpdateRestaurantRequest(
    string Name,
    string City,
    bool IsOpen);
