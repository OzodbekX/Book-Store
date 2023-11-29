using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class Ordering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingId",
                table: "Sale",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "special_discounts",
                table: "Sale",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "discount",
                table: "Book",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vat",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Shipping",
                columns: table => new
                {
                    shipping_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    shipping_state = table.Column<int>(type: "int", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipping", x => x.shipping_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_ShippingId",
                table: "Sale",
                column: "ShippingId");

            migrationBuilder.AddForeignKey(
                name: "FK__Sale__shipping_id__412EB0B6",
                table: "Sale",
                column: "ShippingId",
                principalTable: "Shipping",
                principalColumn: "shipping_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Sale__shipping_id__412EB0B6",
                table: "Sale");

            migrationBuilder.DropTable(
                name: "Shipping");

            migrationBuilder.DropIndex(
                name: "IX_Sale_ShippingId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "ShippingId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "special_discounts",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "discount",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "vat",
                table: "Book");
        }
    }
}
