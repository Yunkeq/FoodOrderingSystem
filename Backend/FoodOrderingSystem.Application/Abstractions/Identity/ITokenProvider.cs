using FoodOrderingSystem.Domain.Entities;

namespace FoodOrderingSystem.Application.Abstractions.Identity;

public interface ITokenProvider
{
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    string GenerateRefreshToken();
}
