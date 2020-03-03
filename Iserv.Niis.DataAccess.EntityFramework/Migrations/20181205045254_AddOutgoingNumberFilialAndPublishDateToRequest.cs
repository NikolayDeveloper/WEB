using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddOutgoingNumberFilialAndPublishDateToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutgoingNumberFilial",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PublishDate",
                table: "Requests",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutgoingNumberFilial",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Requests");
        }
    }
}
