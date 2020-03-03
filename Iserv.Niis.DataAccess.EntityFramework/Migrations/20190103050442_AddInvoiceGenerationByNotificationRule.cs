using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddInvoiceGenerationByNotificationRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoiceGenerationRules_DicDocumentTypes_DocumentTypeId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoiceGenerationRules_DocumentTypeId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "PaymentInvoiceGenerationRules");

            migrationBuilder.CreateTable(
                name: "PaymentInvoiceByNotificationRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    TariffId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInvoiceByNotificationRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentInvoiceByNotificationRules_DicDocumentTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoiceByNotificationRules_DicTariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "DicTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceByNotificationRules_NotificationTypeId",
                table: "PaymentInvoiceByNotificationRules",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceByNotificationRules_TariffId",
                table: "PaymentInvoiceByNotificationRules",
                column: "TariffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentInvoiceByNotificationRules");

            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeId",
                table: "PaymentInvoiceGenerationRules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_DocumentTypeId",
                table: "PaymentInvoiceGenerationRules",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoiceGenerationRules_DicDocumentTypes_DocumentTypeId",
                table: "PaymentInvoiceGenerationRules",
                column: "DocumentTypeId",
                principalTable: "DicDocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
