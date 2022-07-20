using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class addissueassigntable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "feedback",
                table: "issues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "blockManagerId",
                table: "blocks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "departmentId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssignIssue",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    staffId = table.Column<string>(type: "TEXT", nullable: true),
                    issueId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignIssue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AssignIssue_AspNetUsers_staffId",
                        column: x => x.staffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssignIssue_issues_issueId",
                        column: x => x.issueId,
                        principalTable: "issues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    departmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    departmentName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.departmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blocks_blockManagerId",
                table: "blocks",
                column: "blockManagerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_departmentId",
                table: "AspNetUsers",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignIssue_issueId",
                table: "AssignIssue",
                column: "issueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignIssue_staffId",
                table: "AssignIssue",
                column: "staffId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_departments_departmentId",
                table: "AspNetUsers",
                column: "departmentId",
                principalTable: "departments",
                principalColumn: "departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_blocks_AspNetUsers_blockManagerId",
                table: "blocks",
                column: "blockManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_departments_departmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_blocks_AspNetUsers_blockManagerId",
                table: "blocks");

            migrationBuilder.DropTable(
                name: "AssignIssue");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropIndex(
                name: "IX_blocks_blockManagerId",
                table: "blocks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_departmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "feedback",
                table: "issues");

            migrationBuilder.DropColumn(
                name: "blockManagerId",
                table: "blocks");

            migrationBuilder.DropColumn(
                name: "departmentId",
                table: "AspNetUsers");
        }
    }
}
