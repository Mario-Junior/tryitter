using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tryitter.Application.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "CreatedAt", "Email", "Module", "Name", "Password", "Photo", "Status" },
                values: new object[,]
                {
                    { "user1", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user1@test.com", "Computer Science", "User 1", "user1234", "http://local.com/user1.jpg", "Using Tryitter" },
                    { "user2", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user2@test.com", "Computer Science", "User 2", "user1234", "http://local.com/user2.jpg", "Using Tryitter" },
                    { "user3", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user3@test.com", "Computer Science", "User 3", "user1234", "http://local.com/user3.jpg", "Using Tryitter" },
                    { "user4", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user4@test.com", "Computer Science", "User 4", "user1234", "http://local.com/user4.jpg", "Using Tryitter" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CreatedAt", "Image", "Text", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("038b4a0e-bf51-4e8f-95cb-d0d97312cb22"), new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post1.jpg", "Post 1", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user3" },
                    { new Guid("1c529c71-586e-48a7-92bf-b19d92b18540"), new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post2.jpg", "Post 2", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user2" },
                    { new Guid("21ad69ad-63d2-46f5-8bda-8b704a5f211b"), new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post1.jpg", "Post 1", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user1" },
                    { new Guid("31255d37-5778-498a-80b5-9ed9598582e3"), new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post1.jpg", "Post 1", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user2" },
                    { new Guid("8c85b80a-c91d-43fe-b88b-93b502f844c1"), new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post3.jpg", "Post 3", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user1" },
                    { new Guid("e2ab3cd8-e088-4b92-ac1a-dc724919ac93"), new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post2.jpg", "Post 2", new DateTime(2022, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), "user1" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Username",
                table: "Posts",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
