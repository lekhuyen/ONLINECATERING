using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class mi8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Menus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Menus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
