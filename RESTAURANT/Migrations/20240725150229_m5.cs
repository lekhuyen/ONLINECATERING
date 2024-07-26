using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class m5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComboId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ComboId",
                table: "Orders",
                column: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Combos_ComboId",
                table: "Orders",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Combos_ComboId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ComboId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ComboId",
                table: "Orders");
        }
    }
}
