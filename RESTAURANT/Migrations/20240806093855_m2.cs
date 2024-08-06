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
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDishes_Dishes_DishId",
                table: "OrderDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDishes_Orders_OrderId",
                table: "OrderDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDishes",
                table: "OrderDishes");

            migrationBuilder.RenameTable(
                name: "OrderDishes",
                newName: "OrderDish");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDishes_OrderId",
                table: "OrderDish",
                newName: "IX_OrderDish_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDish",
                table: "OrderDish",
                columns: new[] { "DishId", "OrderId" });

            migrationBuilder.CreateTable(
                name: "OOrderDish",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OOrderDish", x => new { x.DishId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OOrderDish_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OOrderDish_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderAppetizers",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AppetizerId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAppetizers", x => new { x.AppetizerId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrderAppetizers_Appetizers_AppetizerId",
                        column: x => x.AppetizerId,
                        principalTable: "Appetizers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderAppetizers_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDesserts",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DessertId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDesserts", x => new { x.DessertId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrderDesserts_Desserts_DessertId",
                        column: x => x.DessertId,
                        principalTable: "Desserts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDesserts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OOrderDish_OrderId",
                table: "OOrderDish",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAppetizers_OrderId",
                table: "OrderAppetizers",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDesserts_OrderId",
                table: "OrderDesserts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDish_Dishes_DishId",
                table: "OrderDish",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDish_Orders_OrderId",
                table: "OrderDish",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDish_Dishes_DishId",
                table: "OrderDish");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDish_Orders_OrderId",
                table: "OrderDish");

            migrationBuilder.DropTable(
                name: "OOrderDish");

            migrationBuilder.DropTable(
                name: "OrderAppetizers");

            migrationBuilder.DropTable(
                name: "OrderDesserts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDish",
                table: "OrderDish");

            migrationBuilder.RenameTable(
                name: "OrderDish",
                newName: "OrderDishes");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDish_OrderId",
                table: "OrderDishes",
                newName: "IX_OrderDishes_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDishes",
                table: "OrderDishes",
                columns: new[] { "DishId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDishes_Dishes_DishId",
                table: "OrderDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDishes_Orders_OrderId",
                table: "OrderDishes",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
