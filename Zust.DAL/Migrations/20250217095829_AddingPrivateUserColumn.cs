using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingPrivateUserColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Users");
        }
    }
}
