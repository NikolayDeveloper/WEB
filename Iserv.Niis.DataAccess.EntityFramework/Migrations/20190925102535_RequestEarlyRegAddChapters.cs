using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RequestEarlyRegAddChapters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChapterOne",
                table: "RequestEarlyRegs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChapterTwo",
                table: "RequestEarlyRegs",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfChapterOne",
                table: "RequestEarlyRegs",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfChapterTwo",
                table: "RequestEarlyRegs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterOne",
                table: "RequestEarlyRegs");

            migrationBuilder.DropColumn(
                name: "ChapterTwo",
                table: "RequestEarlyRegs");

            migrationBuilder.DropColumn(
                name: "DateOfChapterOne",
                table: "RequestEarlyRegs");

            migrationBuilder.DropColumn(
                name: "DateOfChapterTwo",
                table: "RequestEarlyRegs");
        }
    }
}
