using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class PaymentInvoiceDocIdRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Service",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "PaymentInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentService",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_DocumentId",
                table: "PaymentInvoices",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoices_Documents_DocumentId",
                table: "PaymentInvoices",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoices_Documents_DocumentId",
                table: "PaymentInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoices_DocumentId",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "PaymentService",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "Documents",
                nullable: true);
        }
    }
}
