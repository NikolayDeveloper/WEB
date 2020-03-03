using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddTablesDicRequest_ProtectionDocStatusesRoutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DicProtectionDocStatusesRoutes",
                columns: table => new
                {
                    DicProtectionDocStatusId = table.Column<int>(type: "int", nullable: false),
                    DicRouteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicProtectionDocStatusesRoutes", x => new { x.DicProtectionDocStatusId, x.DicRouteId });
                    table.ForeignKey(
                        name: "FK_DicProtectionDocStatusesRoutes_DicProtectionDocStatuses_DicProtectionDocStatusId",
                        column: x => x.DicProtectionDocStatusId,
                        principalTable: "DicProtectionDocStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicProtectionDocStatusesRoutes_DicRoutes_DicRouteId",
                        column: x => x.DicRouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicRequestStatusesRoutes",
                columns: table => new
                {
                    DicRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    DicRouteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicRequestStatusesRoutes", x => new { x.DicRequestStatusId, x.DicRouteId });
                    table.ForeignKey(
                        name: "FK_DicRequestStatusesRoutes_DicRequestStatuses_DicRequestStatusId",
                        column: x => x.DicRequestStatusId,
                        principalTable: "DicRequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRequestStatusesRoutes_DicRoutes_DicRouteId",
                        column: x => x.DicRouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicProtectionDocStatusesRoutes_DicRouteId",
                table: "DicProtectionDocStatusesRoutes",
                column: "DicRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRequestStatusesRoutes_DicRouteId",
                table: "DicRequestStatusesRoutes",
                column: "DicRouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicProtectionDocStatusesRoutes");

            migrationBuilder.DropTable(
                name: "DicRequestStatusesRoutes");
        }
    }
}
