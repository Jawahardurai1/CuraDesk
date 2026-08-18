using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CuraDesk.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFirstLogin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFirstLogin",
                table: "Users");
        }
    }
}
