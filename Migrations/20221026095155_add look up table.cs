using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class addlookuptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lookUp",
                columns: table => new
                {
                    lookUpID = table.Column<Guid>(type: "TEXT", nullable: false),
                    lookUpTypeName = table.Column<string>(type: "TEXT", nullable: true),
                    lookUpTypeCode = table.Column<string>(type: "TEXT", nullable: true),
                    valueString = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lookUp", x => x.lookUpID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lookUp");
        }
    }
}
