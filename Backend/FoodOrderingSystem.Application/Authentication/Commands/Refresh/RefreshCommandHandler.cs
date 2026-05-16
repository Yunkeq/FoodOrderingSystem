using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Identity;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Authentication.Common;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Authentication.Commands.Refresh;

public sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, LoginDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public RefreshCommandHandler(
        IApplicationDbContext dbContext,
        ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<LoginDto>> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = _dbContext.RefreshTokens
            .FirstOrDefault(rt => rt.Token == command.RefreshToken);

        if (refreshToken == null)
        {
            return Result<LoginDto>.Failure(AuthErrors.Unauthorized);
        }

        if (refreshToken.ExpirationDate < DateTime.UtcNow)
        {
            return Result<LoginDto>.Failure(AuthErrors.Unauthorized);
        }

        var user = _dbContext.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == refreshToken.UserId);

        if (user == null)
        {
            return Result<LoginDto>.Failure(AuthErrors.Unauthorized);
        }

        var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user);
        var newRefreshToken = _tokenProvider.GenerateRefreshToken();

        refreshToken.Token = newRefreshToken;
        refreshToken.ExpirationDate = DateTime.UtcNow.AddDays(7);

        return Result<LoginDto>.Success(new LoginDto(accessToken, newRefreshToken));
    }
}
