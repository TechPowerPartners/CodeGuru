using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guard.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArticles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublichedAt",
                table: "Articles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                table: "Articles",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublichedAt",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Articles");
        }
    }
}
