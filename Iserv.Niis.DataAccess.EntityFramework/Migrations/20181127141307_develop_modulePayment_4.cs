using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class develop_modulePayment_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Сounterparty",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "СounterpartyBinOrInn",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNameReturnedPayment",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdvancePayment",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Payer",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerBinOrInn",
                table: "Payments",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentCNumberBVU",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReturnedDate",
                table: "Payments",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeNameReturnedPayment",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsAdvancePayment",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Payer",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PayerBinOrInn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentCNumberBVU",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "Сounterparty",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "СounterpartyBinOrInn",
                table: "Payments",
                maxLength: 12,
                nullable: true);
        }
    }
}
