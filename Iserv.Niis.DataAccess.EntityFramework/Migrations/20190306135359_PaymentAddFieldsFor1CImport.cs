using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class PaymentAddFieldsFor1CImport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ImportedDate",
                table: "Payments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserImportedId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNameImported",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPositionImported",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserImportedId",
                table: "Payments",
                column: "UserImportedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserImportedId",
                table: "Payments",
                column: "UserImportedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserImportedId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserImportedId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ImportedDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserImportedId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserNameImported",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserPositionImported",
                table: "Payments");
        }
    }
}
