using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CaloriesFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Calories",
                table: "Meals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Meal_Calories",
                table: "Meals",
                sql: "Calories > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Meal_Calories",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Meals");
        }
    }
}
