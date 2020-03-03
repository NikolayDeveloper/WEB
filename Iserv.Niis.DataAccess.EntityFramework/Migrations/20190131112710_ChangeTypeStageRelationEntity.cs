using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class ChangeTypeStageRelationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DicBiblioChangeTypeDicRouteStageRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangeTypeId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicBiblioChangeTypeDicRouteStageRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicBiblioChangeTypeDicRouteStageRelations_DicBiblioChangeTypes_ChangeTypeId",
                        column: x => x.ChangeTypeId,
                        principalTable: "DicBiblioChangeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicBiblioChangeTypeDicRouteStageRelations_DicRouteStages_StageId",
                        column: x => x.StageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicBiblioChangeTypeDicRouteStageRelations_ChangeTypeId",
                table: "DicBiblioChangeTypeDicRouteStageRelations",
                column: "ChangeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicBiblioChangeTypeDicRouteStageRelations_StageId",
                table: "DicBiblioChangeTypeDicRouteStageRelations",
                column: "StageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicBiblioChangeTypeDicRouteStageRelations");
        }
    }
}
