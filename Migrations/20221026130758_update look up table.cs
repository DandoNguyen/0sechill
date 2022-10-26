using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class updatelookuptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "textFull",
                table: "lookUp",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "valueInt",
                table: "lookUp",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "textFull",
                table: "lookUp");

            migrationBuilder.DropColumn(
                name: "valueInt",
                table: "lookUp");
        }
    }
}
