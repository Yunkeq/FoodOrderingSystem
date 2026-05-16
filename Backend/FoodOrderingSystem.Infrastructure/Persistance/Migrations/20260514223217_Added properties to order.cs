using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderingSystem.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Addedpropertiestoorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "FoodOrdering",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "FoodOrdering",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "FoodOrdering",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "FoodOrdering",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                schema: "FoodOrdering",
                table: "Orders",
                column: "CustomerId",
                principalSchema: "FoodOrdering",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                schema: "FoodOrdering",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerId",
                schema: "FoodOrdering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "FoodOrdering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "FoodOrdering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "FoodOrdering",
                table: "Orders");
        }
    }
}
