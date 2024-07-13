using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTAURANT.API.Migrations
{
    /// <inheritdoc />
    public partial class mi4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RestaurantImages",
                newName: "RestaurantId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Menus",
                newName: "RestaurantId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Descriptions",
                newName: "RestaurantId");

            migrationBuilder.AlterColumn<int>(
                name: "Warning",
                table: "Restaurants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Menu",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Point",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantImages_RestaurantId",
                table: "RestaurantImages",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RestaurantId",
                table: "Menus",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_RestaurantId",
                table: "Descriptions",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_Restaurants_RestaurantId",
                table: "Descriptions",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Restaurants_RestaurantId",
                table: "Menus",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantImages_Restaurants_RestaurantId",
                table: "RestaurantImages",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_Restaurants_RestaurantId",
                table: "Descriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Restaurants_RestaurantId",
                table: "Menus");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantImages_Restaurants_RestaurantId",
                table: "RestaurantImages");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantImages_RestaurantId",
                table: "RestaurantImages");

            migrationBuilder.DropIndex(
                name: "IX_Menus_RestaurantId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Descriptions_RestaurantId",
                table: "Descriptions");

            migrationBuilder.DropColumn(
                name: "Menu",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Restaurants");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "RestaurantImages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Menus",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Descriptions",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Warning",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
