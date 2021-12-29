using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backened.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    chatId = table.Column<long>(nullable: false),
                    connectionId = table.Column<string>(maxLength: 50, nullable: true),
                    senderId = table.Column<string>(maxLength: 50, nullable: true),
                    receiverId = table.Column<string>(maxLength: 50, nullable: true),
                    message = table.Column<string>(nullable: true),
                    messageStatus = table.Column<string>(maxLength: 10, nullable: true),
                    messageDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    isPrivate = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.chatId);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(maxLength: 50, nullable: true),
                    userPassword = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Signups",
                columns: table => new
                {
                    userId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(maxLength: 50, nullable: true),
                    userPassword = table.Column<string>(maxLength: 50, nullable: true),
                    userEmail = table.Column<string>(maxLength: 50, nullable: true),
                    loginStatus = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signups", x => x.userId);
                });

            migrationBuilder.CreateIndex(
                name: "NonClusteredIndex-20210902-114105",
                table: "Chats",
                columns: new[] { "senderId", "receiverId" });

            migrationBuilder.CreateIndex(
                name: "IX_Logins_userName",
                table: "Logins",
                column: "userName",
                unique: true,
                filter: "[userName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Signups_userEmail",
                table: "Signups",
                column: "userEmail",
                unique: true,
                filter: "[userEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Signups_userName",
                table: "Signups",
                column: "userName",
                unique: true,
                filter: "[userName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Signups");
        }
    }
}
