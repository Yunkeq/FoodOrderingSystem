using FoodOrderingSystem.Api.Contracts.Auth;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Authentication.Commands.Login;
using FoodOrderingSystem.Application.Authentication.Commands.Refresh;
using FoodOrderingSystem.Application.Authentication.Commands.Register;
using FoodOrderingSystem.Application.Authentication.Common;
using FoodOrderingSystem.Application.Common.Options;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FoodOrderingSystem.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";
    private const string AccessTokenCookieName = "accessToken";
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly ICommandHandler<RegisterCommand, Guid> _registerCommandHandler;
    private readonly ICommandHandler<LoginCommand, LoginDto> _loginCommandHandler;
    private readonly ICommandHandler<RefreshCommand, LoginDto> _refreshCommandHandler;

    public AuthController(
        IOptions<JwtOptions> jwtOptions,
        ICommandHandler<RegisterCommand, Guid> registerCommandHandler,
        ICommandHandler<LoginCommand, LoginDto> loginCommandHandler,
        ICommandHandler<RefreshCommand, LoginDto> refreshCommandHandler)
    {
        _jwtOptions = jwtOptions;
        _registerCommandHandler = registerCommandHandler;
        _loginCommandHandler = loginCommandHandler;
        _refreshCommandHandler = refreshCommandHandler;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _loginCommandHandler.Handle(new LoginCommand(request.Email, request.Password), cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        Response.Cookies.Append(AccessTokenCookieName, result.Value!.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.Value.AccessTokenExpirationMinutes),
        });

        Response.Cookies.Append(RefreshTokenCookieName, result.Value!.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpirationDays),
        });

        return NoContent();
    }

    [HttpPost("logout")]
    public IActionResult Logout(CancellationToken cancellationToken)
    {
        Response.Cookies.Delete(AccessTokenCookieName);
        Response.Cookies.Delete(RefreshTokenCookieName);

        return NoContent();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> LoginWithRefresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized();
        }

        var result = await _refreshCommandHandler.Handle(new RefreshCommand(refreshToken), cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        Response.Cookies.Append(AccessTokenCookieName, result.Value!.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.Value.AccessTokenExpirationMinutes),
        });

        Response.Cookies.Append(RefreshTokenCookieName, result.Value!.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpirationDays),
        });

        return NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _registerCommandHandler.Handle(new RegisterCommand(request.Email, request.Password), cancellationToken);
        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        return NoContent();
    }

    private IActionResult ToProblemDetails(Error error)
    {
        var statusCode = error.ErrorCode switch
        {
            ErrorCode.InvalidUserCredentials => StatusCodes.Status401Unauthorized,
            ErrorCode.UserNotFound => StatusCodes.Status404NotFound,
            ErrorCode.Validation => StatusCodes.Status400BadRequest,
            ErrorCode.UserAlreadyExists => StatusCodes.Status400BadRequest,
            _ => throw new ArgumentException("Unexpected error code", nameof(error))
        };

        return Problem(
            statusCode: statusCode,
            title: error.ErrorCode.ToString(),
            detail: error.Message);
    }
}
