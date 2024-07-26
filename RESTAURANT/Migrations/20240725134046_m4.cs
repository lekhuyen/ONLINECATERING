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
            migrationBuilder.DropForeignKey(
                name: "FK_ComboAppetizer_Appetizers_AppetizerId",
                table: "ComboAppetizer");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboAppetizer_Combos_ComboId",
                table: "ComboAppetizer");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboDessert_Combos_ComboId",
                table: "ComboDessert");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboDessert_Desserts_DessertId",
                table: "ComboDessert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboDessert",
                table: "ComboDessert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboAppetizer",
                table: "ComboAppetizer");

            migrationBuilder.RenameTable(
                name: "ComboDessert",
                newName: "ComboDesserts");

            migrationBuilder.RenameTable(
                name: "ComboAppetizer",
                newName: "ComboAppetizers");

            migrationBuilder.RenameIndex(
                name: "IX_ComboDessert_DessertId",
                table: "ComboDesserts",
                newName: "IX_ComboDesserts_DessertId");

            migrationBuilder.RenameIndex(
                name: "IX_ComboAppetizer_AppetizerId",
                table: "ComboAppetizers",
                newName: "IX_ComboAppetizers_AppetizerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboDesserts",
                table: "ComboDesserts",
                columns: new[] { "ComboId", "DessertId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboAppetizers",
                table: "ComboAppetizers",
                columns: new[] { "ComboId", "AppetizerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ComboAppetizers_Appetizers_AppetizerId",
                table: "ComboAppetizers",
                column: "AppetizerId",
                principalTable: "Appetizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboAppetizers_Combos_ComboId",
                table: "ComboAppetizers",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboDesserts_Combos_ComboId",
                table: "ComboDesserts",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboDesserts_Desserts_DessertId",
                table: "ComboDesserts",
                column: "DessertId",
                principalTable: "Desserts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboAppetizers_Appetizers_AppetizerId",
                table: "ComboAppetizers");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboAppetizers_Combos_ComboId",
                table: "ComboAppetizers");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboDesserts_Combos_ComboId",
                table: "ComboDesserts");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboDesserts_Desserts_DessertId",
                table: "ComboDesserts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboDesserts",
                table: "ComboDesserts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboAppetizers",
                table: "ComboAppetizers");

            migrationBuilder.RenameTable(
                name: "ComboDesserts",
                newName: "ComboDessert");

            migrationBuilder.RenameTable(
                name: "ComboAppetizers",
                newName: "ComboAppetizer");

            migrationBuilder.RenameIndex(
                name: "IX_ComboDesserts_DessertId",
                table: "ComboDessert",
                newName: "IX_ComboDessert_DessertId");

            migrationBuilder.RenameIndex(
                name: "IX_ComboAppetizers_AppetizerId",
                table: "ComboAppetizer",
                newName: "IX_ComboAppetizer_AppetizerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboDessert",
                table: "ComboDessert",
                columns: new[] { "ComboId", "DessertId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboAppetizer",
                table: "ComboAppetizer",
                columns: new[] { "ComboId", "AppetizerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ComboAppetizer_Appetizers_AppetizerId",
                table: "ComboAppetizer",
                column: "AppetizerId",
                principalTable: "Appetizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboAppetizer_Combos_ComboId",
                table: "ComboAppetizer",
                column: "ComboId",
                principalTable: "Combos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboDessert_Combos_ComboId",
                table: "ComboDessert",
                column: "ComboId",
                principalTable: "Combos",
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
    }
}
