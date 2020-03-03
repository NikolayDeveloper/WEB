using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class Add_DicRouteStage_NextStage_Relation_To_PaymentInvoiceChargingRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextStageId",
                table: "PaymentInvoiceChargingRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceChargingRules_NextStageId",
                table: "PaymentInvoiceChargingRules",
                column: "NextStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceChargingRules_DicRouteStages_NextStageId",
                table: "PaymentInvoiceChargingRules",
                column: "NextStageId",
                principalTable: "DicRouteStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceChargingRules_DicRouteStages_NextStageId",
                table: "PaymentInvoiceChargingRules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoiceChargingRules_NextStageId",
                table: "PaymentInvoiceChargingRules");

            migrationBuilder.DropColumn(
                name: "NextStageId",
                table: "PaymentInvoiceChargingRules");
        }
    }
}
