using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class addnewfaclityandbookingtask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookingTasks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateOfBooking = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TimeLevelOfBooking = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    isAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    userID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookingTasks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_bookingTasks_AspNetUsers_userID",
                        column: x => x.userID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "publicFacilities",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    typeOfPublic = table.Column<string>(type: "TEXT", nullable: true),
                    facilityCode = table.Column<string>(type: "TEXT", nullable: true),
                    BookingTaskID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publicFacilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_publicFacilities_bookingTasks_BookingTaskID",
                        column: x => x.BookingTaskID,
                        principalTable: "bookingTasks",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookingTasks_userID",
                table: "bookingTasks",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_publicFacilities_BookingTaskID",
                table: "publicFacilities",
                column: "BookingTaskID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "publicFacilities");

            migrationBuilder.DropTable(
                name: "bookingTasks");
        }
    }
}
