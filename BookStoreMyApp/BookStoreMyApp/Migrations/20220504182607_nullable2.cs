using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class nullable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "middle_name",
                table: "User",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                defaultValueSql: "('UNKNOWN')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldDefaultValueSql: "('UNKNOWN')");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "User",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: true,
                defaultValueSql: "('UNKNOWN')",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30,
                oldDefaultValueSql: "('UNKNOWN')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "middle_name",
                table: "User",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValueSql: "('UNKNOWN')",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValueSql: "('UNKNOWN')");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "User",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                defaultValueSql: "('UNKNOWN')",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true,
                oldDefaultValueSql: "('UNKNOWN')");
        }
    }
}
