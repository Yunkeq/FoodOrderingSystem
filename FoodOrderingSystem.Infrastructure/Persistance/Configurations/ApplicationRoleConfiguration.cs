using FoodOrderingSystem.Domain.Entities;
using FoodOrderingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderingSystem.Infrastructure.Persistance.Configurations;

public sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData(
            new ApplicationRole
            {
                Id = Guid.Parse("beed0937-74ed-411e-bbe3-843019837c15"),
                Name = UserRole.Admin.ToString(),
                NormalizedName = UserRole.Admin.ToString().ToUpperInvariant(),
            },
            new ApplicationRole
            {
                Id = Guid.Parse("4eace38b-28b5-4414-8afb-66648ff47fa5"),
                Name = UserRole.Customer.ToString(),
                NormalizedName = UserRole.Customer.ToString().ToUpperInvariant(),
            });
    }
}
