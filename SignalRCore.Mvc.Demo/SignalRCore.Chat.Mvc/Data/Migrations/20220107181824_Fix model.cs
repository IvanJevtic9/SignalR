using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRCore.Chat.Mvc.Data.Migrations
{
    public partial class Fixmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectedUsers_AspNetUsers_UserId",
                table: "ConnectedUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ConnectedUsers",
                newName: "ChatUser");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectedUsers_UserId",
                table: "ConnectedUsers",
                newName: "IX_ConnectedUsers_ChatUser");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectedUsers_AspNetUsers_ChatUser",
                table: "ConnectedUsers",
                column: "ChatUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectedUsers_AspNetUsers_ChatUser",
                table: "ConnectedUsers");

            migrationBuilder.RenameColumn(
                name: "ChatUser",
                table: "ConnectedUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectedUsers_ChatUser",
                table: "ConnectedUsers",
                newName: "IX_ConnectedUsers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectedUsers_AspNetUsers_UserId",
                table: "ConnectedUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
