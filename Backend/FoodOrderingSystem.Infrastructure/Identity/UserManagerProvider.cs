using FoodOrderingSystem.Application.Abstractions.Identity;
using FoodOrderingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodOrderingSystem.Infrastructure.Identity;

public sealed class UserManagerProvider : IUserManagerProvider
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagerProvider(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task AddToRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task CreateUser(ApplicationUser user, string password)
    {
        await _userManager.CreateAsync(user, password);
    }

    public async Task<ICollection<string>> GetUserRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}
