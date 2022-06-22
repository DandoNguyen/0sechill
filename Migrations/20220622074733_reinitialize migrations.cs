using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class reinitializemigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    isPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    isChild = table.Column<bool>(type: "INTEGER", nullable: false),
                    parentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    authorId = table.Column<string>(type: "TEXT", nullable: true),
                    authorsId = table.Column<string>(type: "TEXT", nullable: true),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    issuesID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_comments_AspNetUsers_authorsId",
                        column: x => x.authorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_comments_issues_issuesID",
                        column: x => x.issuesID,
                        principalTable: "issues",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_authorsId",
                table: "comments",
                column: "authorsId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_issuesID",
                table: "comments",
                column: "issuesID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");
        }
    }
}
