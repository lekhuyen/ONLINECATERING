using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class mi10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuImages_Menus_MenuId",
                table: "MenuImages");

            migrationBuilder.DropIndex(
                name: "IX_MenuImages_MenuId",
                table: "MenuImages");

            migrationBuilder.AddColumn<string>(
                name: "MenuImage",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuImage",
                table: "Menus");

            migrationBuilder.CreateIndex(
                name: "IX_MenuImages_MenuId",
                table: "MenuImages",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuImages_Menus_MenuId",
                table: "MenuImages",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
