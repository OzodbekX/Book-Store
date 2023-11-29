using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class aaaa2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "rating",
                table: "Book",
                type: "real",
                nullable: false,
                defaultValue: 1f,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "Book",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 1f);
        }
    }
}
