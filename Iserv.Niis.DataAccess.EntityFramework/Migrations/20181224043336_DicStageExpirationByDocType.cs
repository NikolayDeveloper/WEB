using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DicStageExpirationByDocType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoRouteStageEvents_DicRouteStages_RequestStageId",
                table: "AutoRouteStageEvents");

            migrationBuilder.DropIndex(
                name: "IX_AutoRouteStageEvents_RequestStageId",
                table: "AutoRouteStageEvents");

            migrationBuilder.DropColumn(
                name: "RequestStageId",
                table: "AutoRouteStageEvents");

            migrationBuilder.CreateTable(
                name: "DicStageExpirationByDocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    ExpirationType = table.Column<int>(type: "int", nullable: false),
                    ExpirationValue = table.Column<short>(type: "smallint", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RouteStageId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicStageExpirationByDocTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicStageExpirationByDocTypes_DicDocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicStageExpirationByDocTypes_DicRouteStages_RouteStageId",
                        column: x => x.RouteStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicStageExpirationByDocTypes_DocumentTypeId",
                table: "DicStageExpirationByDocTypes",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicStageExpirationByDocTypes_RouteStageId",
                table: "DicStageExpirationByDocTypes",
                column: "RouteStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicStageExpirationByDocTypes");

            migrationBuilder.AddColumn<int>(
                name: "RequestStageId",
                table: "AutoRouteStageEvents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageEvents_RequestStageId",
                table: "AutoRouteStageEvents",
                column: "RequestStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutoRouteStageEvents_DicRouteStages_RequestStageId",
                table: "AutoRouteStageEvents",
                column: "RequestStageId",
                principalTable: "DicRouteStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
