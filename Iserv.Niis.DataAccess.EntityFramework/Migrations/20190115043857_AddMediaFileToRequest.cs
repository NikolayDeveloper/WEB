using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddMediaFileToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaFileId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_MediaFileId",
                table: "Requests",
                column: "MediaFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Attachments_MediaFileId",
                table: "Requests",
                column: "MediaFileId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Attachments_MediaFileId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_MediaFileId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Requests");
        }
    }
}
