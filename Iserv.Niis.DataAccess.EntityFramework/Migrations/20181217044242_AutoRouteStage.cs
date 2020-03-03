using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AutoRouteStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoRouteStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentStageId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NextStageId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRouteStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoRouteStages_DicRouteStages_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AutoRouteStages_DicRouteStages_NextStageId",
                        column: x => x.NextStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentWorkflowViewers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentWorkflowId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentWorkflowViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentWorkflowViewers_DocumentWorkflows_DocumentWorkflowId",
                        column: x => x.DocumentWorkflowId,
                        principalTable: "DocumentWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentWorkflowViewers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AutoRouteStageEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AutoRouteStageId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRouteStageEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoRouteStageEvents_AutoRouteStages_AutoRouteStageId",
                        column: x => x.AutoRouteStageId,
                        principalTable: "AutoRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AutoRouteStageEvents_DicPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "DicPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AutoRouteStageEvents_DicDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AutoRouteStageViewers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AutoRouteStageId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRouteStageViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoRouteStageViewers_AutoRouteStages_AutoRouteStageId",
                        column: x => x.AutoRouteStageId,
                        principalTable: "AutoRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AutoRouteStageViewers_DicPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "DicPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AutoRouteStageViewers_DicProtectionDocTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageEvents_AutoRouteStageId",
                table: "AutoRouteStageEvents",
                column: "AutoRouteStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageEvents_PositionId",
                table: "AutoRouteStageEvents",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageEvents_TypeId",
                table: "AutoRouteStageEvents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStages_CurrentStageId",
                table: "AutoRouteStages",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStages_NextStageId",
                table: "AutoRouteStages",
                column: "NextStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageViewers_AutoRouteStageId",
                table: "AutoRouteStageViewers",
                column: "AutoRouteStageId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageViewers_PositionId",
                table: "AutoRouteStageViewers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRouteStageViewers_TypeId",
                table: "AutoRouteStageViewers",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflowViewers_DocumentWorkflowId",
                table: "DocumentWorkflowViewers",
                column: "DocumentWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflowViewers_UserId",
                table: "DocumentWorkflowViewers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoRouteStageEvents");

            migrationBuilder.DropTable(
                name: "AutoRouteStageViewers");

            migrationBuilder.DropTable(
                name: "DocumentWorkflowViewers");

            migrationBuilder.DropTable(
                name: "AutoRouteStages");
        }
    }
}
