using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guard.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Articles_ArticleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ArticleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserComments",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Article = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserComments", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "UserLikes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikes", x => x.Guid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserComments");

            migrationBuilder.DropTable(
                name: "UserLikes");

            migrationBuilder.AddColumn<Guid>(
                name: "ArticleId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ArticleId",
                table: "Users",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Articles_ArticleId",
                table: "Users",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }
    }
}
