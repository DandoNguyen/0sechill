using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class updatefilePathtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.CreateTable(
                name: "filePaths",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    filePath = table.Column<string>(type: "TEXT", nullable: true),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    issuesID = table.Column<Guid>(type: "TEXT", nullable: true),
                    userId = table.Column<string>(type: "TEXT", nullable: true),
                    usersId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filePaths", x => x.ID);
                    table.ForeignKey(
                        name: "FK_filePaths_AspNetUsers_usersId",
                        column: x => x.usersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_filePaths_issues_issuesID",
                        column: x => x.issuesID,
                        principalTable: "issues",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_filePaths_issuesID",
                table: "filePaths",
                column: "issuesID");

            migrationBuilder.CreateIndex(
                name: "IX_filePaths_usersId",
                table: "filePaths",
                column: "usersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filePaths");

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    filePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.ID);
                    table.ForeignKey(
                        name: "FK_files_issues_issueId",
                        column: x => x.issueId,
                        principalTable: "issues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_files_issueId",
                table: "files",
                column: "issueId");
        }
    }
}
