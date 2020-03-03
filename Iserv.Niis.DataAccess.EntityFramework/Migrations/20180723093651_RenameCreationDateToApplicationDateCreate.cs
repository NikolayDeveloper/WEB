using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RenameCreationDateToApplicationDateCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "СreationDate",
                table: "Contracts");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ApplicationDateCreate",
                table: "Contracts",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationDateCreate",
                table: "Contracts");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "СreationDate",
                table: "Contracts",
                nullable: true);
        }
    }
}
