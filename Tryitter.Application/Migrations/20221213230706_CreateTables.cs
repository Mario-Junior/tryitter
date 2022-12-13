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
                    { "user1", new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "user1@test.com", "Computer Science", "User 1", "user1234", "http://local.com/user1.jpg", "Using Tryitter" },
                    { "user2", new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "user2@test.com", "Computer Science", "User 2", "user1234", "http://local.com/user2.jpg", "Using Tryitter" },
                    { "user3", new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "user3@test.com", "Computer Science", "User 3", "user1234", "http://local.com/user3.jpg", "Using Tryitter" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CreatedAt", "Image", "Text", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { new Guid("2d180bbb-3eb8-43ec-b891-b8c088ed8ba1"), new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post1.jpg", "Post 1", new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "user1" },
                    { new Guid("3c3b3cda-8fc9-4049-adae-63599b849eb4"), new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post1.jpg", "Post 1", new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "user2" },
                    { new Guid("50091f8c-d738-4df9-b3cd-a4dad58382bb"), new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "http://local.com/post2.jpg", "Post 2", new DateTime(2022, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), "user1" }
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
