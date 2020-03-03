using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class MaterialIncomingControlFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ControlDate",
                table: "Documents",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ControlMark",
                table: "Documents",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOutOfControl",
                table: "Documents",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHasPaymentDocument",
                table: "Documents",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OutOfControl",
                table: "Documents",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionExtensionControlDate",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlDate",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ControlMark",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DateOutOfControl",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IsHasPaymentDocument",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "OutOfControl",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ResolutionExtensionControlDate",
                table: "Documents");
        }
    }
}
