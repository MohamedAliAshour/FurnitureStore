using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureStore.Migrations
{
    /// <inheritdoc />
    public partial class Initialqweqwe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedItems_Users",
                table: "PurchasedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WishList_Users",
                table: "WishList");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_WishList_UserId",
                table: "WishList");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedItems_UserId",
                table: "PurchasedItems");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PurchasedItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PurchasedItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishList_UserId",
                table: "WishList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_UserId",
                table: "PurchasedItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedItems_Users",
                table: "PurchasedItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_WishList_Users",
                table: "WishList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "User_ID");
        }
    }
}
