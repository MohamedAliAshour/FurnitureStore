using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureStore.Migrations
{
    /// <inheritdoc />
    public partial class Itemsimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "images",
                table: "Items",
                type: "int",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldMaxLength: 2147483647);

            migrationBuilder.CreateTable(
                name: "tblImage",
                columns: table => new
                {
                    Images_ID = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    items_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblImage", x => x.Images_ID);
                    table.ForeignKey(
                        name: "FK_tblImages_Items",
                        column: x => x.items_ID,
                        principalTable: "Items",
                        principalColumn: "Items_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_images",
                table: "Items",
                column: "images");

            migrationBuilder.CreateIndex(
                name: "IX_tblImage_items_ID",
                table: "tblImage",
                column: "items_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_tblImages",
                table: "Items",
                column: "images",
                principalTable: "tblImage",
                principalColumn: "Images_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_tblImages",
                table: "Items");

            migrationBuilder.DropTable(
                name: "tblImage");

            migrationBuilder.DropIndex(
                name: "IX_Items_images",
                table: "Items");

            migrationBuilder.AlterColumn<byte[]>(
                name: "images",
                table: "Items",
                type: "varbinary(max)",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 2147483647);
        }
    }
}
