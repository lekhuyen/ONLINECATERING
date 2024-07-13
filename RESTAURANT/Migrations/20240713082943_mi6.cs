using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class mi6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Ratings_RatingId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_RatingId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "Ratings");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalRating",
                table: "Restaurants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Ratings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "Ratings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RestaurantId",
                table: "Ratings",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantId",
                table: "Ratings",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Restaurants_RestaurantId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RestaurantId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "Restaurants");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalRating",
                table: "Ratings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RatingId",
                table: "Restaurants",
                column: "RatingId",
                unique: true,
                filter: "[RatingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Ratings_RatingId",
                table: "Restaurants",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id");
        }
    }
}
