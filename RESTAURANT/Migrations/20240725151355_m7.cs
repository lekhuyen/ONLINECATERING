using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class m7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComboId",
                table: "Promotions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ComboId",
                table: "Promotions",
                column: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Combos_ComboId",
                table: "Promotions",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Combos_ComboId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_ComboId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "ComboId",
                table: "Promotions");
        }
    }
}
