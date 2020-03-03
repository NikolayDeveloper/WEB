using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class PaymentInvoiceNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfChangingChargedPaymentInvoice",
                table: "PaymentInvoices",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfDeletingChargedPaymentInvoice",
                table: "PaymentInvoices",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeAndPositonWhoChangedChargedPaymentInvoice",
                table: "PaymentInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeAndPositonWhoDeleteChargedPaymentInvoice",
                table: "PaymentInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonOfChangingChargedPaymentInvoice",
                table: "PaymentInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonOfDeletingChargedPaymentInvoice",
                table: "PaymentInvoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfChangingChargedPaymentInvoice",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "DateOfDeletingChargedPaymentInvoice",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "EmployeeAndPositonWhoChangedChargedPaymentInvoice",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "EmployeeAndPositonWhoDeleteChargedPaymentInvoice",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "ReasonOfChangingChargedPaymentInvoice",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "ReasonOfDeletingChargedPaymentInvoice",
                table: "PaymentInvoices");
        }
    }
}
