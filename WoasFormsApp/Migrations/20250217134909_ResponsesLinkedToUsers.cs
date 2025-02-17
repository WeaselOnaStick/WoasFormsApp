using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoasFormsApp.Migrations
{
    /// <inheritdoc />
    public partial class ResponsesLinkedToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TemplateFields",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RespondentId",
                table: "Responses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responses_RespondentId",
                table: "Responses",
                column: "RespondentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_AspNetUsers_RespondentId",
                table: "Responses",
                column: "RespondentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_AspNetUsers_RespondentId",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_RespondentId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "RespondentId",
                table: "Responses");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TemplateFields",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
