using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoasFormsApp.Migrations
{
    /// <inheritdoc />
    public partial class SalesForceContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateComments_AspNetUsers_UserId",
                table: "TemplateComments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TemplateComments",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesForceContactId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateComments_AspNetUsers_UserId",
                table: "TemplateComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateComments_AspNetUsers_UserId",
                table: "TemplateComments");

            migrationBuilder.DropColumn(
                name: "SalesForceContactId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TemplateComments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateComments_AspNetUsers_UserId",
                table: "TemplateComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
