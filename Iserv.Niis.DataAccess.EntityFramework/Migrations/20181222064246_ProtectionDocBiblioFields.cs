using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class ProtectionDocBiblioFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColectiveTrademarkParticipantsInfo",
                table: "ProtectionDocs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingNumberFilial",
                table: "ProtectionDocs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PublishDate",
                table: "ProtectionDocs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "ProtectionDocs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpeciesTradeMarkId",
                table: "ProtectionDocs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Translation",
                table: "ProtectionDocs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_SpeciesTradeMarkId",
                table: "ProtectionDocs",
                column: "SpeciesTradeMarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtectionDocs_DicProtectionDocSubTypes_SpeciesTradeMarkId",
                table: "ProtectionDocs",
                column: "SpeciesTradeMarkId",
                principalTable: "DicProtectionDocSubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocs_DicProtectionDocSubTypes_SpeciesTradeMarkId",
                table: "ProtectionDocs");

            migrationBuilder.DropIndex(
                name: "IX_ProtectionDocs_SpeciesTradeMarkId",
                table: "ProtectionDocs");

            migrationBuilder.DropColumn(
                name: "ColectiveTrademarkParticipantsInfo",
                table: "ProtectionDocs");

            migrationBuilder.DropColumn(
                name: "OutgoingNumberFilial",
                table: "ProtectionDocs");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "ProtectionDocs");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "ProtectionDocs");

            migrationBuilder.DropColumn(
                name: "SpeciesTradeMarkId",
                table: "ProtectionDocs");

            migrationBuilder.DropColumn(
                name: "Translation",
                table: "ProtectionDocs");
        }
    }
}
