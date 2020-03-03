using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddFieldIntoRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpeciesTradeMarkId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SpeciesTradeMarkId",
                table: "Requests",
                column: "SpeciesTradeMarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_DicProtectionDocSubTypes_SpeciesTradeMarkId",
                table: "Requests",
                column: "SpeciesTradeMarkId",
                principalTable: "DicProtectionDocSubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_DicProtectionDocSubTypes_SpeciesTradeMarkId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SpeciesTradeMarkId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SpeciesTradeMarkId",
                table: "Requests");
        }
    }
}
