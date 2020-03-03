using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DicDocumentTypeGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentTypeGroupId",
                table: "RouteStageOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DicDocumentTypeGroups",
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
                    table.PrimaryKey("PK_DicDocumentTypeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicDocumentTypeGroupTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentTypeGroupId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDocumentTypeGroupTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicDocumentTypeGroupTypes_DicDocumentTypeGroups_DocumentTypeGroupId",
                        column: x => x.DocumentTypeGroupId,
                        principalTable: "DicDocumentTypeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicDocumentTypeGroupTypes_DicDocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteStageOrders_DocumentTypeGroupId",
                table: "RouteStageOrders",
                column: "DocumentTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDocumentTypeGroupTypes_DocumentTypeGroupId",
                table: "DicDocumentTypeGroupTypes",
                column: "DocumentTypeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDocumentTypeGroupTypes_DocumentTypeId",
                table: "DicDocumentTypeGroupTypes",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStageOrders_DicDocumentTypeGroups_DocumentTypeGroupId",
                table: "RouteStageOrders",
                column: "DocumentTypeGroupId",
                principalTable: "DicDocumentTypeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteStageOrders_DicDocumentTypeGroups_DocumentTypeGroupId",
                table: "RouteStageOrders");

            migrationBuilder.DropTable(
                name: "DicDocumentTypeGroupTypes");

            migrationBuilder.DropTable(
                name: "DicDocumentTypeGroups");

            migrationBuilder.DropIndex(
                name: "IX_RouteStageOrders_DocumentTypeGroupId",
                table: "RouteStageOrders");

            migrationBuilder.DropColumn(
                name: "DocumentTypeGroupId",
                table: "RouteStageOrders");
        }
    }
}
