using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodOrderingSystem.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class addedseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "FoodOrdering",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4eace38b-28b5-4414-8afb-66648ff47fa5"), "1d6d0c8a-9b65-4f20-8c43-4ed8a6d5f4aa", "Customer", "CUSTOMER" },
                    { new Guid("beed0937-74ed-411e-bbe3-843019837c15"), "b0c4f1c4-7e0f-4d6b-9d4f-4b2d7f7b2a11", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                schema: "FoodOrdering",
                table: "MenuItems",
                columns: new[] { "Id", "IsAvailable", "Name", "Price", "RestaurantId" },
                values: new object[,]
                {
                    { new Guid("0a1b2c3d-4e5f-4a6b-9c8d-7e6f5a4b3c2d"), true, "Garlic Naan", 3.25m, new Guid("a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e") },
                    { new Guid("1e7d5d3a-0b6c-4e0c-9a9b-c3d2e1f0a9b8"), true, "Chicken Quesadilla", 11.49m, new Guid("c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b") },
                    { new Guid("2a6f0c1f-0a6e-4d0e-9c91-2f5d6a4d94fb"), true, "Pepperoni Pizza", 14.49m, new Guid("2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1") },
                    { new Guid("3c9d2a1f-7b8e-4d2a-9c6f-2a1e7b8c9d0e"), false, "Chips & Salsa", 4.50m, new Guid("c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b") },
                    { new Guid("4d1a3e2b-6df1-4e48-9c58-0e6f0d8c0db1"), true, "California Roll", 9.25m, new Guid("7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c") },
                    { new Guid("5f4e3d2c-1b0a-4c9d-8e7f-6a5b4c3d2e1f"), true, "Crispy Fries", 4.99m, new Guid("f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f") },
                    { new Guid("6b2c8a6e-6a80-4a6a-9c9e-b2c35d7d9c12"), true, "Carnitas Tacos (3)", 10.99m, new Guid("c4b0f9c8-0b12-4e5a-a754-8f1f1e2c0d7b") },
                    { new Guid("7d6c5b4a-3f2e-4d1c-9b8a-0e1f2a3b4c5d"), true, "Chana Masala", 12.49m, new Guid("a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e") },
                    { new Guid("9a4a44c5-1a9f-4d16-8bda-7e4f0c9d3d6e"), true, "Salmon Nigiri (6 pcs)", 13.50m, new Guid("7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c") },
                    { new Guid("9f8e7d6c-5b4a-4c3d-9e2f-1a0b2c3d4e5f"), true, "Chocolate Milkshake", 6.49m, new Guid("f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f") },
                    { new Guid("b3c9a1d2-0e4f-4a2b-9b8c-7d6e5f4a3b2c"), true, "Chicken Tikka Masala", 15.99m, new Guid("a19f0a6c-3a2b-4f4a-9c4b-2b2c3b1a9f8e") },
                    { new Guid("b7b9d0fd-9cfd-4f6a-9b9a-4f3d1c8b3d2a"), true, "Margherita Pizza", 12.99m, new Guid("2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1") },
                    { new Guid("c8d7e6f5-4a3b-4c2d-9e1f-0a9b8c7d6e5f"), true, "Classic Cheeseburger", 13.99m, new Guid("f6a2d3b1-5c7e-4a3d-9f3a-0c7b2a1d4e6f") },
                    { new Guid("d2f5c4d8-5c7c-4f6c-bf2a-2b4d8a9c8f10"), true, "Garlic Knots", 5.99m, new Guid("2d9d6c23-1f2a-4b26-8cf4-8f8d2407c9a1") },
                    { new Guid("f0f0b9d2-3b4b-4e7c-8b9a-5b2d0c1a7e4f"), true, "Miso Soup", 3.75m, new Guid("7e3b9c6a-8f1d-4d2a-b5d7-1b728d1a4f3c") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4eace38b-28b5-4414-8afb-66648ff47fa5"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("beed0937-74ed-411e-bbe3-843019837c15"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("0a1b2c3d-4e5f-4a6b-9c8d-7e6f5a4b3c2d"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("1e7d5d3a-0b6c-4e0c-9a9b-c3d2e1f0a9b8"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("2a6f0c1f-0a6e-4d0e-9c91-2f5d6a4d94fb"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("3c9d2a1f-7b8e-4d2a-9c6f-2a1e7b8c9d0e"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("4d1a3e2b-6df1-4e48-9c58-0e6f0d8c0db1"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("5f4e3d2c-1b0a-4c9d-8e7f-6a5b4c3d2e1f"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("6b2c8a6e-6a80-4a6a-9c9e-b2c35d7d9c12"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("7d6c5b4a-3f2e-4d1c-9b8a-0e1f2a3b4c5d"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("9a4a44c5-1a9f-4d16-8bda-7e4f0c9d3d6e"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("9f8e7d6c-5b4a-4c3d-9e2f-1a0b2c3d4e5f"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("b3c9a1d2-0e4f-4a2b-9b8c-7d6e5f4a3b2c"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("b7b9d0fd-9cfd-4f6a-9b9a-4f3d1c8b3d2a"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("c8d7e6f5-4a3b-4c2d-9e1f-0a9b8c7d6e5f"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("d2f5c4d8-5c7c-4f6c-bf2a-2b4d8a9c8f10"));

            migrationBuilder.DeleteData(
                schema: "FoodOrdering",
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: new Guid("f0f0b9d2-3b4b-4e7c-8b9a-5b2d0c1a7e4f"));
        }
    }
}
