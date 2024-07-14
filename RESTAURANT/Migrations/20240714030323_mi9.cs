using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class mi9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ingredient",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ingredient",
                table: "Menus");
        }
    }
}
