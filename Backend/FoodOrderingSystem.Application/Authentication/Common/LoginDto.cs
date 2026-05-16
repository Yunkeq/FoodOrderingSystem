namespace FoodOrderingSystem.Application.Authentication.Common;

public sealed record LoginDto(
string AccessToken,
string RefreshToken);
