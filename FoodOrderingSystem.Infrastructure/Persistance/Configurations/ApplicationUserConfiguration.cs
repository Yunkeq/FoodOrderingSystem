using FoodOrderingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderingSystem.Infrastructure.Persistance.Configurations;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(au => au.Email)
            .IsRequired();

        builder.Property(au => au.NormalizedEmail)
            .IsRequired();
    }
}
