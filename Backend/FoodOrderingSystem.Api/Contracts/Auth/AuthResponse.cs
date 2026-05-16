namespace FoodOrderingSystem.Api.Contracts.Auth;

/// <summary>
/// Returned by Login/Register/Refresh.
/// If you put tokens in cookies, you can omit tokens from the body and keep only metadata.
/// </summary>
public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken,
    string TokenType,
    int ExpiresInSeconds);