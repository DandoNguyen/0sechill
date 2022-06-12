using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class updateIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_issues_AspNetUsers_authorId1",
                table: "issues");

            migrationBuilder.DropIndex(
                name: "IX_issues_authorId1",
                table: "issues");

            migrationBuilder.DropColumn(
                name: "authorId1",
                table: "issues");

            migrationBuilder.AlterColumn<string>(
                name: "authorId",
                table: "issues",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_issues_authorId",
                table: "issues",
                column: "authorId");

            migrationBuilder.AddForeignKey(
                name: "FK_issues_AspNetUsers_authorId",
                table: "issues",
                column: "authorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_issues_AspNetUsers_authorId",
                table: "issues");

            migrationBuilder.DropIndex(
                name: "IX_issues_authorId",
                table: "issues");

            migrationBuilder.AlterColumn<Guid>(
                name: "authorId",
                table: "issues",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "authorId1",
                table: "issues",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_issues_authorId1",
                table: "issues",
                column: "authorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_issues_AspNetUsers_authorId1",
                table: "issues",
                column: "authorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
