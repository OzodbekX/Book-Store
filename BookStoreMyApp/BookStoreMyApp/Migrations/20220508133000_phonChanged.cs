using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class phonChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Author",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValueSql: "('UNKNOWN')",
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldUnicode: false,
                oldMaxLength: 12,
                oldDefaultValueSql: "('UNKNOWN')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Author",
                type: "varchar(12)",
                unicode: false,
                maxLength: 12,
                nullable: false,
                defaultValueSql: "('UNKNOWN')",
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldDefaultValueSql: "('UNKNOWN')");
        }
    }
}
