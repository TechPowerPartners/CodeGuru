using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace botovskixAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInterview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPassed",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "ToTime",
                table: "Interviews",
                newName: "Date_ToTime");

            migrationBuilder.RenameColumn(
                name: "FromTime",
                table: "Interviews",
                newName: "Date_FromTime");

            migrationBuilder.RenameColumn(
                name: "ToRole",
                table: "Interviews",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Interviews",
                newName: "Date_Date");

            migrationBuilder.RenameColumn(
                name: "FromRole",
                table: "Interviews",
                newName: "Role_To");

            migrationBuilder.AddColumn<DateTime>(
                name: "ComlitionDate",
                table: "Interviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Interviews",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResultStatus",
                table: "Interviews",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role_From",
                table: "Interviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComlitionDate",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "ResultStatus",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "Role_From",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "Date_ToTime",
                table: "Interviews",
                newName: "ToTime");

            migrationBuilder.RenameColumn(
                name: "Date_FromTime",
                table: "Interviews",
                newName: "FromTime");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Interviews",
                newName: "ToRole");

            migrationBuilder.RenameColumn(
                name: "Role_To",
                table: "Interviews",
                newName: "FromRole");

            migrationBuilder.RenameColumn(
                name: "Date_Date",
                table: "Interviews",
                newName: "StartDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsPassed",
                table: "Interviews",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Interviews",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
