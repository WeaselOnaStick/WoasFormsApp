using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoasFormsApp.Migrations
{
    /// <inheritdoc />
    public partial class SalesForceAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalesForceAccountId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesForceAccountId",
                table: "AspNetUsers");
        }
    }
}
