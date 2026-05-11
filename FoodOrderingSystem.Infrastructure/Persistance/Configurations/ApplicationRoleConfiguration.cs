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
                ConcurrencyStamp = "b0c4f1c4-7e0f-4d6b-9d4f-4b2d7f7b2a11",
            },
            new ApplicationRole
            {
                Id = Guid.Parse("4eace38b-28b5-4414-8afb-66648ff47fa5"),
                Name = UserRole.Customer.ToString(),
                NormalizedName = UserRole.Customer.ToString().ToUpperInvariant(),
                ConcurrencyStamp = "1d6d0c8a-9b65-4f20-8c43-4ed8a6d5f4aa",
            }
        );
    }
}
