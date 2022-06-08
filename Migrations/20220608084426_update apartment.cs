using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class updateapartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "apartmentName",
                table: "apartments",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "apartmentName",
                table: "apartments");
        }
    }
}
