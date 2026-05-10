namespace FoodOrderingSystem.Api.Contracts.Restaurants;

public sealed record CreateRestaurantRequest(
    string Name,
    string City,
    bool IsOpen);
