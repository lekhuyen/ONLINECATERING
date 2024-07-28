using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LobbyId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LobbyId",
                table: "Orders",
                column: "LobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Lobbies_LobbyId",
                table: "Orders",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Lobbies_LobbyId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_LobbyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "Orders");
        }
    }
}
