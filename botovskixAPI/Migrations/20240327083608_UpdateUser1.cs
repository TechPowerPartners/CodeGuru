using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace botovskixAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
