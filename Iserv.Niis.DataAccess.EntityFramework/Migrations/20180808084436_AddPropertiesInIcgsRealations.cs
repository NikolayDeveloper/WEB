using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddPropertiesInIcgsRealations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPartialRefused",
                table: "ICGS_Request",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForPartialRefused",
                table: "ICGS_Request",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPartialRefused",
                table: "ICGS_ProtectionDoc",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRefused",
                table: "ICGS_ProtectionDoc",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForPartialRefused",
                table: "ICGS_ProtectionDoc",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPartialRefused",
                table: "ICGS_Request");

            migrationBuilder.DropColumn(
                name: "ReasonForPartialRefused",
                table: "ICGS_Request");

            migrationBuilder.DropColumn(
                name: "IsPartialRefused",
                table: "ICGS_ProtectionDoc");

            migrationBuilder.DropColumn(
                name: "IsRefused",
                table: "ICGS_ProtectionDoc");

            migrationBuilder.DropColumn(
                name: "ReasonForPartialRefused",
                table: "ICGS_ProtectionDoc");
        }
    }
}
