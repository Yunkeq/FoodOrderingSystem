using FoodOrderingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderingSystem.Infrastructure.Persistance.Configurations;

public sealed class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.ToTable("MenuItems");

        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mi => mi.IsAvailable)
            .IsRequired();

        builder.Property(mi => mi.Price)
            .IsRequired();

        builder.HasOne(mi => mi.Restaurant)
            .WithMany()
            .HasForeignKey(mi => mi.RestaurantId);

        builder.HasData(
            // Pizza Palace (New York) - 2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1
            new MenuItem
            {
                Id = new Guid("b7b9d0fd-9cfd-4f6a-9b9a-4f3d1c8b3d2a"),
                RestaurantId = new Guid("2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1"),
                Name = "Margherita Pizza",
                Price = 12.99m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("2a6f0c1f-0a6e-4d0e-9c91-2f5d6a4d94fb"),
                RestaurantId = new Guid("2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1"),
                Name = "Pepperoni Pizza",
                Price = 14.49m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("d2f5c4d8-5c7c-4f6c-bf2a-2b4d8a9c8f10"),
                RestaurantId = new Guid("2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1"),
                Name = "Garlic Knots",
                Price = 5.99m,
                IsAvailable = true,
            },

            // Sushi Spot (Seattle) - 7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c
            new MenuItem
            {
                Id = new Guid("4d1a3e2b-6df1-4e48-9c58-0e6f0d8c0db1"),
                RestaurantId = new Guid("7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c"),
                Name = "California Roll",
                Price = 9.25m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("9a4a44c5-1a9f-4d16-8bda-7e4f0c9d3d6e"),
                RestaurantId = new Guid("7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c"),
                Name = "Salmon Nigiri (6 pcs)",
                Price = 13.50m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("f0f0b9d2-3b4b-4e7c-8b9a-5b2d0c1a7e4f"),
                RestaurantId = new Guid("7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c"),
                Name = "Miso Soup",
                Price = 3.75m,
                IsAvailable = true,
            },

            // Taco Town (Austin) - c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b
            new MenuItem
            {
                Id = new Guid("6b2c8a6e-6a80-4a6a-9c9e-b2c35d7d9c12"),
                RestaurantId = new Guid("c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b"),
                Name = "Carnitas Tacos (3)",
                Price = 10.99m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("1e7d5d3a-0b6c-4e0c-9a9b-c3d2e1f0a9b8"),
                RestaurantId = new Guid("c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b"),
                Name = "Chicken Quesadilla",
                Price = 11.49m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("3c9d2a1f-7b8e-4d2a-9c6f-2a1e7b8c9d0e"),
                RestaurantId = new Guid("c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b"),
                Name = "Chips & Salsa",
                Price = 4.50m,
                IsAvailable = false,
            },

            // Curry Corner (Chicago) - a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e
            new MenuItem
            {
                Id = new Guid("b3c9a1d2-0e4f-4a2b-9b8c-7d6e5f4a3b2c"),
                RestaurantId = new Guid("a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e"),
                Name = "Chicken Tikka Masala",
                Price = 15.99m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("7d6c5b4a-3f2e-4d1c-9b8a-0e1f2a3b4c5d"),
                RestaurantId = new Guid("a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e"),
                Name = "Chana Masala",
                Price = 12.49m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("0a1b2c3d-4e5f-4a6b-9c8d-7e6f5a4b3c2d"),
                RestaurantId = new Guid("a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e"),
                Name = "Garlic Naan",
                Price = 3.25m,
                IsAvailable = true,
            },

            // Burger Barn (Denver) - f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f
            new MenuItem
            {
                Id = new Guid("c8d7e6f5-4a3b-4c2d-9e1f-0a9b8c7d6e5f"),
                RestaurantId = new Guid("f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f"),
                Name = "Classic Cheeseburger",
                Price = 13.99m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("5f4e3d2c-1b0a-4c9d-8e7f-6a5b4c3d2e1f"),
                RestaurantId = new Guid("f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f"),
                Name = "Crispy Fries",
                Price = 4.99m,
                IsAvailable = true,
            },
            new MenuItem
            {
                Id = new Guid("9f8e7d6c-5b4a-4c3d-9e2f-1a0b2c3d4e5f"),
                RestaurantId = new Guid("f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f"),
                Name = "Chocolate Milkshake",
                Price = 6.49m,
                IsAvailable = true,
            });
    }
}
