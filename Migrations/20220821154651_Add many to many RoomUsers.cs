using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class AddmanytomanyRoomUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userConnections");

            migrationBuilder.CreateTable(
                name: "ApplicationUserRoom",
                columns: table => new
                {
                    chatRoomsID = table.Column<Guid>(type: "TEXT", nullable: false),
                    usersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRoom", x => new { x.chatRoomsID, x.usersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoom_AspNetUsers_usersId",
                        column: x => x.usersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoom_chatRooms_chatRoomsID",
                        column: x => x.chatRoomsID,
                        principalTable: "chatRooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRoom_usersId",
                table: "ApplicationUserRoom",
                column: "usersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserRoom");

            migrationBuilder.CreateTable(
                name: "userConnections",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    roomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    userId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userConnections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_userConnections_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_userConnections_chatRooms_roomId",
                        column: x => x.roomId,
                        principalTable: "chatRooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userConnections_roomId",
                table: "userConnections",
                column: "roomId");

            migrationBuilder.CreateIndex(
                name: "IX_userConnections_userId",
                table: "userConnections",
                column: "userId");
        }
    }
}
