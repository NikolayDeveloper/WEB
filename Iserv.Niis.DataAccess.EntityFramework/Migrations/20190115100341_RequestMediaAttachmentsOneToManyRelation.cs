using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RequestMediaAttachmentsOneToManyRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "Attachments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Attachments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_RequestId",
                table: "Attachments",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Requests_RequestId",
                table: "Attachments",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Requests_RequestId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_RequestId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Attachments");

            migrationBuilder.AddColumn<int>(
                name: "MediaFileId",
                table: "Requests",
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
    }
}
