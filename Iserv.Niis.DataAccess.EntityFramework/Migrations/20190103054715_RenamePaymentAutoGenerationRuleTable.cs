using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RenamePaymentAutoGenerationRuleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicDocumentTypes_PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicTariffs_TariffId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentInvoiceByNotificationRules",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.RenameTable(
                name: "PaymentInvoiceByNotificationRules",
                newName: "PaymentInvoiceByPetitionRules");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentInvoiceByNotificationRules_TariffId",
                table: "PaymentInvoiceByPetitionRules",
                newName: "IX_PaymentInvoiceByPetitionRules_TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentInvoiceByNotificationRules_PetitionTypeId",
                table: "PaymentInvoiceByPetitionRules",
                newName: "IX_PaymentInvoiceByPetitionRules_PetitionTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentInvoiceByPetitionRules",
                table: "PaymentInvoiceByPetitionRules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceByPetitionRules_DicDocumentTypes_PetitionTypeId",
                table: "PaymentInvoiceByPetitionRules",
                column: "PetitionTypeId",
                principalTable: "DicDocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceByPetitionRules_DicTariffs_TariffId",
                table: "PaymentInvoiceByPetitionRules",
                column: "TariffId",
                principalTable: "DicTariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceByPetitionRules_DicDocumentTypes_PetitionTypeId",
                table: "PaymentInvoiceByPetitionRules");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceByPetitionRules_DicTariffs_TariffId",
                table: "PaymentInvoiceByPetitionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentInvoiceByPetitionRules",
                table: "PaymentInvoiceByPetitionRules");

            migrationBuilder.RenameTable(
                name: "PaymentInvoiceByPetitionRules",
                newName: "PaymentInvoiceByNotificationRules");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentInvoiceByPetitionRules_TariffId",
                table: "PaymentInvoiceByNotificationRules",
                newName: "IX_PaymentInvoiceByNotificationRules_TariffId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentInvoiceByPetitionRules_PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules",
                newName: "IX_PaymentInvoiceByNotificationRules_PetitionTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentInvoiceByNotificationRules",
                table: "PaymentInvoiceByNotificationRules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicDocumentTypes_PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules",
                column: "PetitionTypeId",
                principalTable: "DicDocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicTariffs_TariffId",
                table: "PaymentInvoiceByNotificationRules",
                column: "TariffId",
                principalTable: "DicTariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
