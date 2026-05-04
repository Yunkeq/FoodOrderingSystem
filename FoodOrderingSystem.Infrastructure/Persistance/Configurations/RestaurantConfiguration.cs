using FoodOrderingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderingSystem.Infrastructure.Persistance.Configurations;

public sealed class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("Restaurants");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.IsOpen)
            .IsRequired();

        builder.HasData(
            new Restaurant
            {
                Id = new Guid("2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1"),
                Name = "Pizza Palace",
                City = "New York",
                IsOpen = true,
            },
            new Restaurant
            {
                Id = new Guid("7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c"),
                Name = "Sushi Spot",
                City = "Seattle",
                IsOpen = true,
            },
            new Restaurant
            {
                Id = new Guid("c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b"),
                Name = "Taco Town",
                City = "Austin",
                IsOpen = false,
            },
            new Restaurant
            {
                Id = new Guid("a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e"),
                Name = "Curry Corner",
                City = "Chicago",
                IsOpen = true,
            },
            new Restaurant
            {
                Id = new Guid("f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f"),
                Name = "Burger Barn",
                City = "Denver",
                IsOpen = true,
            });
    }
}
