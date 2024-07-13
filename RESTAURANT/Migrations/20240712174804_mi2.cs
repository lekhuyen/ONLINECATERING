using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class mi2 : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "RatingId",
                table: "Restaurants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Ratings_RatingId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_RatingId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<int>(
                name: "RatingId",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RatingId",
                table: "Restaurants",
                column: "RatingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Ratings_RatingId",
                table: "Restaurants",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
