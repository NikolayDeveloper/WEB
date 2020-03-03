using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class develop_modulePayment_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CashBalance",
                table: "Payments",
                type: "decimal(18, 2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignCurrency",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentUseAmmountSumm",
                table: "Payments",
                type: "decimal(18, 2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashBalance",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsForeignCurrency",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentUseAmmountSumm",
                table: "Payments");
        }
    }
}
