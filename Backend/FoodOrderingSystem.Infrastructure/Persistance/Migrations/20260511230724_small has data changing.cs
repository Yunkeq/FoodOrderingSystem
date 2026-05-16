using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderingSystem.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class smallhasdatachanging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "FoodOrdering",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4eace38b-28b5-4414-8afb-66648ff47fa5"),
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                schema: "FoodOrdering",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("beed0937-74ed-411e-bbe3-843019837c15"),
                column: "ConcurrencyStamp",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "FoodOrdering",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4eace38b-28b5-4414-8afb-66648ff47fa5"),
                column: "ConcurrencyStamp",
                value: "1d6d0c8a-9b65-4f20-8c43-4ed8a6d5f4aa");

            migrationBuilder.UpdateData(
                schema: "FoodOrdering",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("beed0937-74ed-411e-bbe3-843019837c15"),
                column: "ConcurrencyStamp",
                value: "b0c4f1c4-7e0f-4d6b-9d4f-4b2d7f7b2a11");
        }
    }
}
