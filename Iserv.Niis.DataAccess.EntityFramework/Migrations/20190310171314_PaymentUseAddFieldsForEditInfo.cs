using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class PaymentUseAddFieldsForEditInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EditClearedPaymentDate",
                table: "PaymentUses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditClearedPaymentEmployeeName",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditClearedPaymentReason",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditClearedPaymentDate",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "EditClearedPaymentEmployeeName",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "EditClearedPaymentReason",
                table: "PaymentUses");
        }
    }
}
