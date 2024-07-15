using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USER.API.Migrations
{
    /// <inheritdoc />
    public partial class mi6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserBooking_UserId",
                table: "UserBooking",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooking_Users_UserId",
                table: "UserBooking",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBooking_Users_UserId",
                table: "UserBooking");

            migrationBuilder.DropIndex(
                name: "IX_UserBooking_UserId",
                table: "UserBooking");
        }
    }
}
