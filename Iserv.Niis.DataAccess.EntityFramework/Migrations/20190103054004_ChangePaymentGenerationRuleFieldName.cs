using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class ChangePaymentGenerationRuleFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicDocumentTypes_NotificationTypeId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoiceByNotificationRules_NotificationTypeId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.DropColumn(
                name: "NotificationTypeId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.AddColumn<int>(
                name: "PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceByNotificationRules_PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules",
                column: "PetitionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicDocumentTypes_PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules",
                column: "PetitionTypeId",
                principalTable: "DicDocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicDocumentTypes_PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoiceByNotificationRules_PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.DropColumn(
                name: "PetitionTypeId",
                table: "PaymentInvoiceByNotificationRules");

            migrationBuilder.AddColumn<int>(
                name: "NotificationTypeId",
                table: "PaymentInvoiceByNotificationRules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceByNotificationRules_NotificationTypeId",
                table: "PaymentInvoiceByNotificationRules",
                column: "NotificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceByNotificationRules_DicDocumentTypes_NotificationTypeId",
                table: "PaymentInvoiceByNotificationRules",
                column: "NotificationTypeId",
                principalTable: "DicDocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
