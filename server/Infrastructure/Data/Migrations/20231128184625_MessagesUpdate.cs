using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class MessagesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_RecipientId",
                table: "PrivateMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_SenderId",
                table: "PrivateMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateMessage",
                table: "PrivateMessage");

            migrationBuilder.RenameTable(
                name: "PrivateMessage",
                newName: "PrivateMessages");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessage_SenderId",
                table: "PrivateMessages",
                newName: "IX_PrivateMessages_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessage_RecipientId",
                table: "PrivateMessages",
                newName: "IX_PrivateMessages_RecipientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateMessages",
                table: "PrivateMessages",
                column: "PrivateMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_RecipientId",
                table: "PrivateMessages",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_SenderId",
                table: "PrivateMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_RecipientId",
                table: "PrivateMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_AspNetUsers_SenderId",
                table: "PrivateMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateMessages",
                table: "PrivateMessages");

            migrationBuilder.RenameTable(
                name: "PrivateMessages",
                newName: "PrivateMessage");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessages_SenderId",
                table: "PrivateMessage",
                newName: "IX_PrivateMessage_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateMessages_RecipientId",
                table: "PrivateMessage",
                newName: "IX_PrivateMessage_RecipientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateMessage",
                table: "PrivateMessage",
                column: "PrivateMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_RecipientId",
                table: "PrivateMessage",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessage_AspNetUsers_SenderId",
                table: "PrivateMessage",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
