using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Items_ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    images = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Items_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Cart_id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Items_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Cart_id);
                    table.ForeignKey(
                        name: "FK_Carts_Items",
                        column: x => x.Items_ID,
                        principalTable: "Items",
                        principalColumn: "Items_ID");
                    table.ForeignKey(
                        name: "FK_Carts_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateTable(
                name: "PurchasedItems",
                columns: table => new
                {
                    PurchasedItems_ID = table.Column<int>(type: "int", nullable: false),
                    DatePurchased = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Items_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedItems", x => x.PurchasedItems_ID);
                    table.ForeignKey(
                        name: "FK_PurchasedItems_Items",
                        column: x => x.Items_ID,
                        principalTable: "Items",
                        principalColumn: "Items_ID");
                    table.ForeignKey(
                        name: "FK_PurchasedItems_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateTable(
                name: "WishList",
                columns: table => new
                {
                    WishList_ID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Items_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishList", x => x.WishList_ID);
                    table.ForeignKey(
                        name: "FK_WishList_Items",
                        column: x => x.Items_ID,
                        principalTable: "Items",
                        principalColumn: "Items_ID");
                    table.ForeignKey(
                        name: "FK_WishList_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_Items_ID",
                table: "Carts",
                column: "Items_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_Items_ID",
                table: "PurchasedItems",
                column: "Items_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_UserId",
                table: "PurchasedItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_Items_ID",
                table: "WishList",
                column: "Items_ID");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_UserId",
                table: "WishList",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "PurchasedItems");

            migrationBuilder.DropTable(
                name: "WishList");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
