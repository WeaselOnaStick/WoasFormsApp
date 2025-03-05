using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoasFormsApp.Migrations
{
    /// <inheritdoc />
    public partial class JiraCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JiraAccountId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JiraAccountId",
                table: "AspNetUsers");
        }
    }
}
