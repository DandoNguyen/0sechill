using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class testmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_publicFacilities_bookingTasks_BookingTaskID",
                table: "publicFacilities");

            migrationBuilder.DropIndex(
                name: "IX_publicFacilities_BookingTaskID",
                table: "publicFacilities");

            migrationBuilder.DropColumn(
                name: "BookingTaskID",
                table: "publicFacilities");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "userHistories",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicFacilityID",
                table: "bookingTasks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bookingTasks_PublicFacilityID",
                table: "bookingTasks",
                column: "PublicFacilityID");

            migrationBuilder.AddForeignKey(
                name: "FK_bookingTasks_publicFacilities_PublicFacilityID",
                table: "bookingTasks",
                column: "PublicFacilityID",
                principalTable: "publicFacilities",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookingTasks_publicFacilities_PublicFacilityID",
                table: "bookingTasks");

            migrationBuilder.DropIndex(
                name: "IX_bookingTasks_PublicFacilityID",
                table: "bookingTasks");

            migrationBuilder.DropColumn(
                name: "PublicFacilityID",
                table: "bookingTasks");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "userHistories",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BookingTaskID",
                table: "publicFacilities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_publicFacilities_BookingTaskID",
                table: "publicFacilities",
                column: "BookingTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_publicFacilities_bookingTasks_BookingTaskID",
                table: "publicFacilities",
                column: "BookingTaskID",
                principalTable: "bookingTasks",
                principalColumn: "ID");
        }
    }
}
