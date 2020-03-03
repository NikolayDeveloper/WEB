using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddPropertyAddressKzAndAddressEnInCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressEn",
                table: "RequestCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressKz",
                table: "RequestCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressEn",
                table: "ProtectionDocCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressKz",
                table: "ProtectionDocCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressEn",
                table: "DicCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressKz",
                table: "DicCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressEn",
                table: "ContractCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressKz",
                table: "ContractCustomers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressEn",
                table: "RequestCustomers");

            migrationBuilder.DropColumn(
                name: "AddressKz",
                table: "RequestCustomers");

            migrationBuilder.DropColumn(
                name: "AddressEn",
                table: "ProtectionDocCustomers");

            migrationBuilder.DropColumn(
                name: "AddressKz",
                table: "ProtectionDocCustomers");

            migrationBuilder.DropColumn(
                name: "AddressEn",
                table: "DicCustomers");

            migrationBuilder.DropColumn(
                name: "AddressKz",
                table: "DicCustomers");

            migrationBuilder.DropColumn(
                name: "AddressEn",
                table: "ContractCustomers");

            migrationBuilder.DropColumn(
                name: "AddressKz",
                table: "ContractCustomers");
        }
    }
}
