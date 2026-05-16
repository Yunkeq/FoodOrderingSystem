using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Identity;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Authentication.Common;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.Options;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FoodOrderingSystem.Application.Authentication.Commands.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserManagerProvider _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IOptions<JwtOptions> _jwtOptions;

    public LoginCommandHandler(
        IApplicationDbContext dbContext,
        IUserManagerProvider userManager,
        ITokenProvider tokenProvider,
        IOptions<JwtOptions> jwtOptions)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _jwtOptions = jwtOptions;
    }

    public async Task<Result<LoginDto>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Email == command.Email);

        if (user is null)
        {
            return Result<LoginDto>.Failure(UserErrors.UserNotFound(command.Email));
        }

        if (!await _userManager.CheckPasswordAsync(user, command.Password))
        {
            return Result<LoginDto>.Failure(UserErrors.InvalidUserCredentials(command.Email));
        }

        var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var refreshEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpirationDays),
        };

        await _dbContext.RefreshTokens.AddAsync(refreshEntity, cancellationToken);

        return Result<LoginDto>.Success(new LoginDto(accessToken, refreshToken));
    }
}
