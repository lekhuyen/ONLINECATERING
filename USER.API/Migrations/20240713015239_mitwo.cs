using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USER.API.Migrations
{
    /// <inheritdoc />
    public partial class mitwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "FavoriteLists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "FavoriteLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
