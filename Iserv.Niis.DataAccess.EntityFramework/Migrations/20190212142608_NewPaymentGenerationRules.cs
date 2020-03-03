using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class NewPaymentGenerationRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceGenerationRules_DicReceiveTypes_ReceiveTypeId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceGenerationRules_DicProtectionDocSubTypes_SpeciesTrademarkId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoiceGenerationRules_ReceiveTypeId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoiceGenerationRules_SpeciesTrademarkId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropColumn(
                name: "IcgsCount",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropColumn(
                name: "ReceiveTypeId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropColumn(
                name: "SpeciesTrademarkId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.AddColumn<bool>(
                name: "IsCtm",
                table: "DicTariffs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMoreThanIcgsThreshold",
                table: "DicTariffs",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCtm",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "IsMoreThanIcgsThreshold",
                table: "DicTariffs");

            migrationBuilder.AddColumn<int>(
                name: "IcgsCount",
                table: "PaymentInvoiceGenerationRules",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiveTypeId",
                table: "PaymentInvoiceGenerationRules",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpeciesTrademarkId",
                table: "PaymentInvoiceGenerationRules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_ReceiveTypeId",
                table: "PaymentInvoiceGenerationRules",
                column: "ReceiveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_SpeciesTrademarkId",
                table: "PaymentInvoiceGenerationRules",
                column: "SpeciesTrademarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceGenerationRules_DicReceiveTypes_ReceiveTypeId",
                table: "PaymentInvoiceGenerationRules",
                column: "ReceiveTypeId",
                principalTable: "DicReceiveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceGenerationRules_DicProtectionDocSubTypes_SpeciesTrademarkId",
                table: "PaymentInvoiceGenerationRules",
                column: "SpeciesTrademarkId",
                principalTable: "DicProtectionDocSubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
