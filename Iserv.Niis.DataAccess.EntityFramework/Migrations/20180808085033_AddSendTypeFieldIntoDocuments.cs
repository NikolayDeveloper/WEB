using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddSendTypeFieldIntoDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SendTypeId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SendTypeId",
                table: "Documents",
                column: "SendTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DicReceiveTypes_SendTypeId",
                table: "Documents",
                column: "SendTypeId",
                principalTable: "DicReceiveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DicReceiveTypes_SendTypeId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_SendTypeId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "SendTypeId",
                table: "Documents");
        }
    }
}
