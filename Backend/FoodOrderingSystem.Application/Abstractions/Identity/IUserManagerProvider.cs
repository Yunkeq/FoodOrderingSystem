using FoodOrderingSystem.Domain.Entities;

namespace FoodOrderingSystem.Application.Abstractions.Identity;

public interface IUserManagerProvider
{
    Task<ICollection<string>> GetUserRolesAsync(ApplicationUser user);
    Task CreateUser(ApplicationUser user, string password);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task AddToRoleAsync(ApplicationUser user, string role);
}
