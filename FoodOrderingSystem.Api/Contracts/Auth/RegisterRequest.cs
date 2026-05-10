namespace FoodOrderingSystem.Api.Contracts.Auth;

public sealed record RegisterRequest(
    string Email,
    string Password);