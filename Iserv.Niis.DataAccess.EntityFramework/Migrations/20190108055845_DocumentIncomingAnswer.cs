using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DocumentIncomingAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncomingAnswerId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_IncomingAnswerId",
                table: "Documents",
                column: "IncomingAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Documents_IncomingAnswerId",
                table: "Documents",
                column: "IncomingAnswerId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Documents_IncomingAnswerId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_IncomingAnswerId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IncomingAnswerId",
                table: "Documents");
        }
    }
}
