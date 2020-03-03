using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RouteStageOrderGroupToClassification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteStageOrders_DicDocumentTypeGroups_DocumentTypeGroupId",
                table: "RouteStageOrders");

            migrationBuilder.DropIndex(
                name: "IX_RouteStageOrders_DocumentTypeGroupId",
                table: "RouteStageOrders");

            migrationBuilder.DropColumn(
                name: "DocumentTypeGroupId",
                table: "RouteStageOrders");

            migrationBuilder.AddColumn<int>(
                name: "ClassificationId",
                table: "RouteStageOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteStageOrders_ClassificationId",
                table: "RouteStageOrders",
                column: "ClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStageOrders_DicDocumentClassifications_ClassificationId",
                table: "RouteStageOrders",
                column: "ClassificationId",
                principalTable: "DicDocumentClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteStageOrders_DicDocumentClassifications_ClassificationId",
                table: "RouteStageOrders");

            migrationBuilder.DropIndex(
                name: "IX_RouteStageOrders_ClassificationId",
                table: "RouteStageOrders");

            migrationBuilder.DropColumn(
                name: "ClassificationId",
                table: "RouteStageOrders");

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeGroupId",
                table: "RouteStageOrders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteStageOrders_DocumentTypeGroupId",
                table: "RouteStageOrders",
                column: "DocumentTypeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStageOrders_DicDocumentTypeGroups_DocumentTypeGroupId",
                table: "RouteStageOrders",
                column: "DocumentTypeGroupId",
                principalTable: "DicDocumentTypeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
