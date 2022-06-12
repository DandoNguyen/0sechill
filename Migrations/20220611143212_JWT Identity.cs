using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class JWTIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    cateName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "issues",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    status = table.Column<string>(type: "TEXT", nullable: true),
                    createdDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    lastModifiedDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    privacy = table.Column<bool>(type: "INTEGER", nullable: false),
                    cateId = table.Column<Guid>(type: "TEXT", nullable: false),
                    categoryID = table.Column<Guid>(type: "TEXT", nullable: true),
                    authorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    authorId1 = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_issues_AspNetUsers_authorId1",
                        column: x => x.authorId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_issues_categories_categoryID",
                        column: x => x.categoryID,
                        principalTable: "categories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    filePath = table.Column<string>(type: "TEXT", nullable: true),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_issues_authorId1",
                table: "issues",
                column: "authorId1");

            migrationBuilder.CreateIndex(
                name: "IX_issues_categoryID",
                table: "issues",
                column: "categoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "issues");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
