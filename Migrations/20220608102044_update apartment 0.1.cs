using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class updateapartment01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ísFurnitureAvailable",
                table: "apartments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ísFurnitureAvailable",
                table: "apartments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
