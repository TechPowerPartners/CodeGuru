using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guard.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserLikesAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
