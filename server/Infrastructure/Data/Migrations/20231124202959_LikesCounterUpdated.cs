using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikesCounterUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Comments");
        }
    }
}
