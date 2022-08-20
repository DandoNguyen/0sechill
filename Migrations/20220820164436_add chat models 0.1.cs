using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class addchatmodels01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatMessages_chatRooms_RoomID",
                table: "chatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_userConnections_chatRooms_RoomID",
                table: "userConnections");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "userConnections",
                newName: "roomId");

            migrationBuilder.RenameIndex(
                name: "IX_userConnections_RoomID",
                table: "userConnections",
                newName: "IX_userConnections_roomId");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "chatMessages",
                newName: "roomId");

            migrationBuilder.RenameIndex(
                name: "IX_chatMessages_RoomID",
                table: "chatMessages",
                newName: "IX_chatMessages_roomId");

            migrationBuilder.AlterColumn<Guid>(
                name: "roomId",
                table: "userConnections",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "roomId",
                table: "chatMessages",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_chatMessages_chatRooms_roomId",
                table: "chatMessages",
                column: "roomId",
                principalTable: "chatRooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userConnections_chatRooms_roomId",
                table: "userConnections",
                column: "roomId",
                principalTable: "chatRooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatMessages_chatRooms_roomId",
                table: "chatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_userConnections_chatRooms_roomId",
                table: "userConnections");

            migrationBuilder.RenameColumn(
                name: "roomId",
                table: "userConnections",
                newName: "RoomID");

            migrationBuilder.RenameIndex(
                name: "IX_userConnections_roomId",
                table: "userConnections",
                newName: "IX_userConnections_RoomID");

            migrationBuilder.RenameColumn(
                name: "roomId",
                table: "chatMessages",
                newName: "RoomID");

            migrationBuilder.RenameIndex(
                name: "IX_chatMessages_roomId",
                table: "chatMessages",
                newName: "IX_chatMessages_RoomID");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomID",
                table: "userConnections",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomID",
                table: "chatMessages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_chatMessages_chatRooms_RoomID",
                table: "chatMessages",
                column: "RoomID",
                principalTable: "chatRooms",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_userConnections_chatRooms_RoomID",
                table: "userConnections",
                column: "RoomID",
                principalTable: "chatRooms",
                principalColumn: "ID");
        }
    }
}
