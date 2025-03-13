using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CuisinesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CuisineId",
                table: "Meals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Cuisines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuisines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_CuisineId",
                table: "Meals",
                column: "CuisineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Cuisines_CuisineId",
                table: "Meals",
                column: "CuisineId",
                principalTable: "Cuisines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Cuisines_CuisineId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "Cuisines");

            migrationBuilder.DropIndex(
                name: "IX_Meals_CuisineId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "CuisineId",
                table: "Meals");
        }
    }
}
