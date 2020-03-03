using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DicPositionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DicPositionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicPositionTypes", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO DicPositionTypes(Code, DateCreate, DateUpdate, NameRu, IsDeleted) VALUES (N'temp', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'temp', 0)");

            migrationBuilder.AddColumn<int>(
                name: "PositionTypeId",
                table: "DicPositions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_DicPositions_PositionTypeId",
                table: "DicPositions",
                column: "PositionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DicPositions_DicPositionTypes_PositionTypeId",
                table: "DicPositions",
                column: "PositionTypeId",
                principalTable: "DicPositionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DicPositions_DicPositionTypes_PositionTypeId",
                table: "DicPositions");

            migrationBuilder.DropTable(
                name: "DicPositionTypes");

            migrationBuilder.DropIndex(
                name: "IX_DicPositions_PositionTypeId",
                table: "DicPositions");

            migrationBuilder.DropColumn(
                name: "PositionTypeId",
                table: "DicPositions");
        }
    }
}
