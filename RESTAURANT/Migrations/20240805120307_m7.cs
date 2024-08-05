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
                name: "BeverageId",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BeverageId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Comments",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Beverages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeverageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    BeverageImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CountRatings = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beverages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComboBeverage",
                columns: table => new
                {
                    ComboId = table.Column<int>(type: "int", nullable: false),
                    BeverageId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboBeverage", x => new { x.ComboId, x.BeverageId });
                    table.ForeignKey(
                        name: "FK_ComboBeverage_Beverages_BeverageId",
                        column: x => x.BeverageId,
                        principalTable: "Beverages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboBeverage_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BeverageId",
                table: "Ratings",
                column: "BeverageId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BeverageId",
                table: "Comments",
                column: "BeverageId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboBeverage_BeverageId",
                table: "ComboBeverage",
                column: "BeverageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Beverages_BeverageId",
                table: "Comments",
                column: "BeverageId",
                principalTable: "Beverages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Beverages_BeverageId",
                table: "Ratings",
                column: "BeverageId",
                principalTable: "Beverages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Beverages_BeverageId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Beverages_BeverageId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "ComboBeverage");

            migrationBuilder.DropTable(
                name: "Beverages");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_BeverageId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Comments_BeverageId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "BeverageId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "BeverageId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Comments");
        }
    }
}
