using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeOfPublic",
                table: "publicFacilities",
                newName: "typeFacil");

            migrationBuilder.RenameColumn(
                name: "facilityCode",
                table: "publicFacilities",
                newName: "facilCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeFacil",
                table: "publicFacilities",
                newName: "typeOfPublic");

            migrationBuilder.RenameColumn(
                name: "facilCode",
                table: "publicFacilities",
                newName: "facilityCode");
        }
    }
}
