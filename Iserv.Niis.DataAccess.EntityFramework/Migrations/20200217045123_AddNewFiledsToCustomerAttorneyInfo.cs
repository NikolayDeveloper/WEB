using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddNewFiledsToCustomerAttorneyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "CustomerAttorneyInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CertDate",
                table: "CustomerAttorneyInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CertNum",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Iin",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KnowledgeArea",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFirst",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameLast",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameMiddle",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ops",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevalidNote",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "CustomerAttorneyInfos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAttorneyInfos_CountryId",
                table: "CustomerAttorneyInfos",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAttorneyInfos_LocationId",
                table: "CustomerAttorneyInfos",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAttorneyInfos_DicCountries_CountryId",
                table: "CustomerAttorneyInfos",
                column: "CountryId",
                principalTable: "DicCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAttorneyInfos_DicAddresses_LocationId",
                table: "CustomerAttorneyInfos",
                column: "LocationId",
                principalTable: "DicAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAttorneyInfos_DicCountries_CountryId",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAttorneyInfos_DicAddresses_LocationId",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAttorneyInfos_CountryId",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAttorneyInfos_LocationId",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "CertDate",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "CertNum",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Iin",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "KnowledgeArea",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "NameFirst",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "NameLast",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "NameMiddle",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Ops",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "RevalidNote",
                table: "CustomerAttorneyInfos");

            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "CustomerAttorneyInfos");
        }
    }
}
