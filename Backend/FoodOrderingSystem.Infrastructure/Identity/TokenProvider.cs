using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FoodOrderingSystem.Application.Abstractions.Identity;
using FoodOrderingSystem.Application.Common.Options;
using FoodOrderingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FoodOrderingSystem.Infrastructure.Identity;

public sealed class TokenProvider : ITokenProvider
{
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly IUserManagerProvider _userManager;

    public TokenProvider(IOptions<JwtOptions> jwtOptions,IUserManagerProvider userManager)
    {
        _jwtOptions = jwtOptions;
        _userManager = userManager;
    }

    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var key = _jwtOptions.Value.SigningKey;
        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var signingCredentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetUserRolesAsync(user);

        List<Claim> claims = [
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            ..roles.Select(r => new Claim("role", r)),
            new Claim("email", user.Email!),
            ];

        //foreach (var role in roles)
        //{
        //    claims.Add(new("role", role));
        //}

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _jwtOptions.Value.Audience,
            Issuer = _jwtOptions.Value.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt16(_jwtOptions.Value.AccessTokenExpirationMinutes)),
            SigningCredentials = signingCredentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
