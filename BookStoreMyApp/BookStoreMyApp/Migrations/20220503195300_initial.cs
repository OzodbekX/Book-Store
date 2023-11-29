using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreMyApp.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    last_name = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    first_name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    phone = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false, defaultValueSql: "('UNKNOWN')"),
                    address = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    state = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    zip = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    email_address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    job_id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    job_desc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('New Position - title not formalized yet')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.job_id);
                });

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    pub_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    publisher_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    city = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    state = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    country = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, defaultValueSql: "('USA')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Publishe__2515F2220B4D3120", x => x.pub_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValueSql: "('New Position - title not formalized yet')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    store_id = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    store_name = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    store_address = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    city = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    state = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    zip = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.store_id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    type = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false, defaultValueSql: "('UNDECIDED')"),
                    pub_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: true),
                    advance = table.Column<decimal>(type: "money", nullable: true),
                    royalty = table.Column<int>(type: "int", nullable: true),
                    ytd_sales = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    published_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.book_id);
                    table.ForeignKey(
                        name: "FK__Book__pub_id__3D5E1FD2",
                        column: x => x.pub_id,
                        principalTable: "Publisher",
                        principalColumn: "pub_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email_address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    middle_name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    last_name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    role_id = table.Column<short>(type: "smallint", nullable: false, defaultValueSql: "((1))"),
                    PublisherPubId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_id_2", x => x.user_id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK__User__role_id__4316F928",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Publisher_PublisherPubId",
                        column: x => x.PublisherPubId,
                        principalTable: "Publisher",
                        principalColumn: "pub_id");
                });

            migrationBuilder.CreateTable(
                name: "BookAuthor",
                columns: table => new
                {
                    author_id = table.Column<int>(type: "int", nullable: false),
                    book_id = table.Column<int>(type: "int", nullable: false),
                    author_order = table.Column<byte>(type: "tinyint", nullable: true),
                    royalty_percentage = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthor", x => new { x.author_id, x.book_id });
                    table.ForeignKey(
                        name: "FK__BookAutho__autho__3E52440B",
                        column: x => x.author_id,
                        principalTable: "Author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__BookAutho__book___3F466844",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    size = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.id);
                    table.ForeignKey(
                        name: "FK_Picture_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    token_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    admin_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    expiry_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.token_id);
                    table.ForeignKey(
                        name: "FK__RefreshTo__admin___403A8C7D",
                        column: x => x.admin_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    sale_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    store_id = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: false),
                    order_num = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    quantity = table.Column<short>(type: "smallint", nullable: false),
                    pay_terms = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    book_id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.sale_id);
                    table.ForeignKey(
                        name: "FK__Sale__book_id__412EB0B6",
                        column: x => x.book_id,
                        principalTable: "Book",
                        principalColumn: "book_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Sale__store_id__4222D4EF",
                        column: x => x.store_id,
                        principalTable: "Store",
                        principalColumn: "store_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sale_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_pub_id",
                table: "Book",
                column: "pub_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_book_id",
                table: "BookAuthor",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Picture_BookId",
                table: "Picture",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_admin_id",
                table: "RefreshToken",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_book_id",
                table: "Sale",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_store_id",
                table: "Sale",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_UserId",
                table: "Sale",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PublisherPubId",
                table: "User",
                column: "PublisherPubId");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthor");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Publisher");
        }
    }
}
