using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class TasksIsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question_text = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    option1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quis_ball = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.question_id);
                });

            migrationBuilder.CreateTable(
                name: "UserTask",
                columns: table => new
                {
                    UserTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_task_tittle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTask", x => x.UserTaskId);
                });

            migrationBuilder.CreateTable(
                name: "TaskFile",
                columns: table => new
                {
                    file_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    size = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFile", x => x.file_id);
                    table.ForeignKey(
                        name: "FK_TaskFile_UserTask_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTask",
                        principalColumn: "UserTaskId");
                });

            migrationBuilder.CreateTable(
                name: "UserResult",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserTaskId = table.Column<int>(type: "int", nullable: false),
                    result = table.Column<int>(type: "int", nullable: false),
                    task_taype = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResult", x => new { x.UserId, x.UserTaskId });
                    table.ForeignKey(
                        name: "FK__user_id__book___3F466844",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserResult_UserTask_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTask",
                        principalColumn: "UserTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskFile_UserTaskId",
                table: "TaskFile",
                column: "UserTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResult_UserTaskId",
                table: "UserResult",
                column: "UserTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "TaskFile");

            migrationBuilder.DropTable(
                name: "UserResult");

            migrationBuilder.DropTable(
                name: "UserTask");
        }
    }
}
