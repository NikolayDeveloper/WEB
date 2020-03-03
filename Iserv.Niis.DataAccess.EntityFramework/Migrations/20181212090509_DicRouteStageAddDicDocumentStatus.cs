using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DicRouteStageAddDicDocumentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DicDocumentStatusId",
                table: "DicRouteStages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_DicDocumentStatusId",
                table: "DicRouteStages",
                column: "DicDocumentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_DicRouteStages_DicDocumentStatuses_DicDocumentStatusId",
                table: "DicRouteStages",
                column: "DicDocumentStatusId",
                principalTable: "DicDocumentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DicRouteStages_DicDocumentStatuses_DicDocumentStatusId",
                table: "DicRouteStages");

            migrationBuilder.DropIndex(
                name: "IX_DicRouteStages_DicDocumentStatusId",
                table: "DicRouteStages");

            migrationBuilder.DropColumn(
                name: "DicDocumentStatusId",
                table: "DicRouteStages");
        }
    }
}
