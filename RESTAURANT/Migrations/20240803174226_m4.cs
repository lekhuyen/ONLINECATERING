using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class m4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountRatings",
                table: "Dishes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountRatings",
                table: "Desserts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountRatings",
                table: "Appetizers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountRatings",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CountRatings",
                table: "Desserts");

            migrationBuilder.DropColumn(
                name: "CountRatings",
                table: "Appetizers");
        }
    }
}
