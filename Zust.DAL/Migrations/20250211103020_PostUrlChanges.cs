using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PostUrlChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
