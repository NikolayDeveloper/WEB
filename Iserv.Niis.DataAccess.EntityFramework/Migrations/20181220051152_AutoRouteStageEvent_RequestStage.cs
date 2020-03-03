using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AutoRouteStageEvent_RequestStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestStageId",
                table: "AutoRouteStageEvents",
                type: "int",
                nullable: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageEvents_RequestStageId",
                table: "AutoRouteStageEvents",
                column: "RequestStageId");
            
            migrationBuilder.AddForeignKey(
                name: "FK_AutoRouteStageEvents_DicRouteStages_RequestStageId",
                table: "AutoRouteStageEvents",
                column: "RequestStageId",
                principalTable: "DicRouteStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);  
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoRouteStageEvents_DicRouteStages_RequestStageId",
                table: "AutoRouteStageEvents");

            migrationBuilder.DropIndex(
                name: "IX_AutoRouteStageEvents_RequestStageId",
                table: "AutoRouteStageEvents");

            migrationBuilder.DropColumn(
                name: "RequestStageId",
                table: "AutoRouteStageEvents");
        }
    }
}
