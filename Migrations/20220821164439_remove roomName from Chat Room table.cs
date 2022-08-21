using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _0sechill.Migrations
{
    public partial class removeroomNamefromChatRoomtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roomName",
                table: "chatRooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "roomName",
                table: "chatRooms",
                type: "TEXT",
                nullable: true);
        }
    }
}
