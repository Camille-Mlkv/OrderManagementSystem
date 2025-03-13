using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MealTagsDeleteBehaviourModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealTags_Meals_MealId",
                table: "MealTags");

            migrationBuilder.DropForeignKey(
                name: "FK_MealTags_Tags_TagId",
                table: "MealTags");

            migrationBuilder.AddForeignKey(
                name: "FK_MealTags_Meals_MealId",
                table: "MealTags",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealTags_Tags_TagId",
                table: "MealTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealTags_Meals_MealId",
                table: "MealTags");

            migrationBuilder.DropForeignKey(
                name: "FK_MealTags_Tags_TagId",
                table: "MealTags");

            migrationBuilder.AddForeignKey(
                name: "FK_MealTags_Meals_MealId",
                table: "MealTags",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MealTags_Tags_TagId",
                table: "MealTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
