using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class address1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskAddress",
                table: "UserTask",
                newName: "task_address");

            migrationBuilder.AddColumn<string>(
                name: "task_address",
                table: "Question",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "task_address",
                table: "Question");

            migrationBuilder.RenameColumn(
                name: "task_address",
                table: "UserTask",
                newName: "TaskAddress");
        }
    }
}
