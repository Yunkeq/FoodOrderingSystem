using FoodOrderingSystem.Domain.Entities;
using FoodOrderingSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderingSystem.Infrastructure.Identity;

public static class IdentitySeedRunner
{
    private const string AdminEmail = "admin@gmail.com";
    private const string AdminPassword = "zaq1@WSX";
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await EnsureRole(roleManager, UserRole.Admin.ToString());
        await EnsureRole(roleManager, UserRole.Customer.ToString());

        if (string.IsNullOrWhiteSpace(AdminEmail) || string.IsNullOrWhiteSpace(AdminPassword))
        {
            // Roles seeded, but admin user seeding is disabled until credentials are provided.
            return;
        }

        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = AdminEmail,
                UserName = AdminEmail,
                EmailConfirmed = true,
            };

            var createResult = await userManager.CreateAsync(admin, AdminPassword);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException($"Failed to seed admin user. {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, UserRole.Admin.ToString()))
        {
            var addResult = await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
            if (!addResult.Succeeded)
            {
                var errors = string.Join(", ", addResult.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException($"Failed to add admin role. {errors}");
            }
        }
    }

    private static async Task EnsureRole(RoleManager<ApplicationRole> roleManager, string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        var result = await roleManager.CreateAsync(new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = roleName,
            NormalizedName = roleName.ToUpperInvariant(),
        });

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}:{e.Description}"));
            throw new InvalidOperationException($"Failed to seed role '{roleName}'. {errors}");
        }
    }
}