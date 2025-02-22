using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class VideoAndImageMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Posts",
                newName: "MediaUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaUrl",
                table: "Posts",
                newName: "ImageUrl");
        }
    }
}
