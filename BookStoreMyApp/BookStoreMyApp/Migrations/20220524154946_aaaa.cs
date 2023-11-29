using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class aaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "available_addresses",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "available_addresses",
                table: "User");
        }
    }
}
