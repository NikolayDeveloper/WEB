using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RemoveFiledsFromCustomerAttorneyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBeginStop",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "DateCard",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "DateDisCard",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "DateEndStop",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "DatePublic",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "FieldOfActivity",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "FieldOfKnowledge",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "GovReg",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "GovRegDate",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "PaymentOrder",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "PublicRedefine",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Redefine",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "RegCode",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "SomeDate",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "WorkPlace",
                table: "CustomerAttorneyInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateBeginStop",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateCard",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateDisCard",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateEndStop",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DatePublic",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldOfActivity",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldOfKnowledge",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GovReg",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "GovRegDate",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentOrder",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicRedefine",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Redefine",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegCode",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SomeDate",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPlace",
                table: "CustomerAttorneyInfos",
                nullable: true);
        }
    }
}
