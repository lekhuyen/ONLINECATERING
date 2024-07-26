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
            migrationBuilder.CreateTable(
                name: "Appetizer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppetizerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AppetizerImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appetizer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dessert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DessertName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DessertImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dessert", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComboAppetizer",
                columns: table => new
                {
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    AppetizerId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboAppetizer", x => new { x.ComboId, x.AppetizerId });
                    table.ForeignKey(
                        name: "FK_ComboAppetizer_Appetizer_AppetizerId",
                        column: x => x.AppetizerId,
                        principalTable: "Appetizer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboAppetizer_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComboDessert",
                columns: table => new
                {
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    DessertId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboDessert", x => new { x.ComboId, x.DessertId });
                    table.ForeignKey(
                        name: "FK_ComboDessert_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboDessert_Dessert_DessertId",
                        column: x => x.DessertId,
                        principalTable: "Dessert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComboAppetizer_AppetizerId",
                table: "ComboAppetizer",
                column: "AppetizerId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboDessert_DessertId",
                table: "ComboDessert",
                column: "DessertId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboAppetizer");

            migrationBuilder.DropTable(
                name: "ComboDessert");

            migrationBuilder.DropTable(
                name: "Appetizer");

            migrationBuilder.DropTable(
                name: "Dessert");
        }
    }
}
