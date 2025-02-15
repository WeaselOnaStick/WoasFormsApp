using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WoasFormsApp.Migrations
{
    /// <inheritdoc />
    public partial class HiddenFieldsRegistrationDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Templates_TemplateId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateComments_Templates_TemplateId",
                table: "TemplateComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateFields_TemplateFieldType_TypeId",
                table: "TemplateFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateFieldType",
                table: "TemplateFieldType");

            migrationBuilder.RenameTable(
                name: "TemplateFieldType",
                newName: "FieldTypes");

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "TemplateFields",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "TemplateComments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "Responses",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerCheckedBoxes",
                table: "ResponseAnswer",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FieldTypes",
                table: "FieldTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Templates_TemplateId",
                table: "Responses",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateComments_Templates_TemplateId",
                table: "TemplateComments",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateFields_FieldTypes_TypeId",
                table: "TemplateFields",
                column: "TypeId",
                principalTable: "FieldTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Templates_TemplateId",
                table: "Responses");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateComments_Templates_TemplateId",
                table: "TemplateComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateFields_FieldTypes_TypeId",
                table: "TemplateFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FieldTypes",
                table: "FieldTypes");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "TemplateFields");

            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "FieldTypes",
                newName: "TemplateFieldType");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "TemplateComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "Responses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerCheckedBoxes",
                table: "ResponseAnswer",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateFieldType",
                table: "TemplateFieldType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Templates_TemplateId",
                table: "Responses",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateComments_Templates_TemplateId",
                table: "TemplateComments",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateFields_TemplateFieldType_TypeId",
                table: "TemplateFields",
                column: "TypeId",
                principalTable: "TemplateFieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
