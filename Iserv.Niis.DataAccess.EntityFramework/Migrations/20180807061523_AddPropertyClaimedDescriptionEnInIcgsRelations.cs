using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddPropertyClaimedDescriptionEnInIcgsRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClaimedDescriptionEn",
                table: "ICGS_Request",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimedDescriptionEn",
                table: "ICGS_ProtectionDoc",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimedDescriptionEn",
                table: "ICGS_Request");

            migrationBuilder.DropColumn(
                name: "ClaimedDescriptionEn",
                table: "ICGS_ProtectionDoc");
        }
    }
}
