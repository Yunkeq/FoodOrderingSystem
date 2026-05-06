using FoodOrderingSystem.Domain.Entities;

namespace FoodOrderingSystem.Application.Abstractions.Identity;

public interface IUserManagerProvider
{
    Task<List<string>> GetUserRolesAsync(ApplicationUser user);
}
