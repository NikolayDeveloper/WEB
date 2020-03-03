using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddEntityPaymentGeneratorRuleAndRemoveOldExpertSearchFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentInvoiceGenerationRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
                    SpeciesTrademarkId = table.Column<int>(type: "int", nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: true),
                    TariffId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInvoiceGenerationRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentInvoiceGenerationRules_DicDocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoiceGenerationRules_DicReceiveTypes_ReceiveTypeId",
                        column: x => x.ReceiveTypeId,
                        principalTable: "DicReceiveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoiceGenerationRules_DicProtectionDocSubTypes_SpeciesTrademarkId",
                        column: x => x.SpeciesTrademarkId,
                        principalTable: "DicProtectionDocSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoiceGenerationRules_DicRouteStages_StageId",
                        column: x => x.StageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoiceGenerationRules_DicTariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "DicTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_DocumentTypeId",
                table: "PaymentInvoiceGenerationRules",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_ReceiveTypeId",
                table: "PaymentInvoiceGenerationRules",
                column: "ReceiveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_SpeciesTrademarkId",
                table: "PaymentInvoiceGenerationRules",
                column: "SpeciesTrademarkId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_StageId",
                table: "PaymentInvoiceGenerationRules",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoiceGenerationRules_TariffId",
                table: "PaymentInvoiceGenerationRules",
                column: "TariffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentInvoiceGenerationRules");
        }
    }
}
