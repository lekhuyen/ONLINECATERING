using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboAppetizer_Appetizer_AppetizerId",
                table: "ComboAppetizer");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboDessert_Dessert_DessertId",
                table: "ComboDessert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dessert",
                table: "Dessert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appetizer",
                table: "Appetizer");

            migrationBuilder.RenameTable(
                name: "Dessert",
                newName: "Desserts");

            migrationBuilder.RenameTable(
                name: "Appetizer",
                newName: "Appetizers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Desserts",
                table: "Desserts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appetizers",
                table: "Appetizers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComboAppetizer_Appetizers_AppetizerId",
                table: "ComboAppetizer",
                column: "AppetizerId",
                principalTable: "Appetizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboDessert_Desserts_DessertId",
                table: "ComboDessert",
                column: "DessertId",
                principalTable: "Desserts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboAppetizer_Appetizers_AppetizerId",
                table: "ComboAppetizer");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboDessert_Desserts_DessertId",
                table: "ComboDessert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Desserts",
                table: "Desserts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appetizers",
                table: "Appetizers");

            migrationBuilder.RenameTable(
                name: "Desserts",
                newName: "Dessert");

            migrationBuilder.RenameTable(
                name: "Appetizers",
                newName: "Appetizer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dessert",
                table: "Dessert",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appetizer",
                table: "Appetizer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComboAppetizer_Appetizer_AppetizerId",
                table: "ComboAppetizer",
                column: "AppetizerId",
                principalTable: "Appetizer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboDessert_Dessert_DessertId",
                table: "ComboDessert",
                column: "DessertId",
                principalTable: "Dessert",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
