using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoasFormsApp.Migrations
{
    /// <inheritdoc />
    public partial class ReqTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerCheckedBoxes",
                table: "ResponseAnswer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Responses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "AnswerCheckedBox",
                table: "ResponseAnswer",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "AnswerCheckedBox",
                table: "ResponseAnswer");

            migrationBuilder.AddColumn<string>(
                name: "AnswerCheckedBoxes",
                table: "ResponseAnswer",
                type: "TEXT",
                nullable: true);
        }
    }
}
