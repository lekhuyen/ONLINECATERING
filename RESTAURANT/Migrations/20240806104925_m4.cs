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
                name: "FK_OOrderDish_Dishes_DishId",
                table: "OOrderDish");

            migrationBuilder.DropForeignKey(
                name: "FK_OOrderDish_Orders_OrderId",
                table: "OOrderDish");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDish_Dishes_DishId",
                table: "OrderDish");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDish_Orders_OrderId",
                table: "OrderDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDish",
                table: "OrderDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OOrderDish",
                table: "OOrderDish");

            migrationBuilder.RenameTable(
                name: "OrderDish",
                newName: "OrderDishes");

            migrationBuilder.RenameTable(
                name: "OOrderDish",
                newName: "OOrderDishes");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDish_OrderId",
                table: "OrderDishes",
                newName: "IX_OrderDishes_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OOrderDish_OrderId",
                table: "OOrderDishes",
                newName: "IX_OOrderDishes_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDishes",
                table: "OrderDishes",
                columns: new[] { "DishId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OOrderDishes",
                table: "OOrderDishes",
                columns: new[] { "DishId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OOrderDishes_Dishes_DishId",
                table: "OOrderDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OOrderDishes_Orders_OrderId",
                table: "OOrderDishes",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OOrderDishes_Dishes_DishId",
                table: "OOrderDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_OOrderDishes_Orders_OrderId",
                table: "OOrderDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDishes_Dishes_DishId",
                table: "OrderDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDishes_Orders_OrderId",
                table: "OrderDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDishes",
                table: "OrderDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OOrderDishes",
                table: "OOrderDishes");

            migrationBuilder.RenameTable(
                name: "OrderDishes",
                newName: "OrderDish");

            migrationBuilder.RenameTable(
                name: "OOrderDishes",
                newName: "OOrderDish");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDishes_OrderId",
                table: "OrderDish",
                newName: "IX_OrderDish_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OOrderDishes_OrderId",
                table: "OOrderDish",
                newName: "IX_OOrderDish_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDish",
                table: "OrderDish",
                columns: new[] { "DishId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OOrderDish",
                table: "OOrderDish",
                columns: new[] { "DishId", "OrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OOrderDish_Dishes_DishId",
                table: "OOrderDish",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OOrderDish_Orders_OrderId",
                table: "OOrderDish",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
