using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class PaymentAddedBlockedInfoFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BlockedAmount",
                table: "Payments",
                type: "decimal(18, 2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BlockedDate",
                table: "Payments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNameBlockedPayment",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedAmount",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BlockedDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EmployeeNameBlockedPayment",
                table: "Payments");
        }
    }
}
