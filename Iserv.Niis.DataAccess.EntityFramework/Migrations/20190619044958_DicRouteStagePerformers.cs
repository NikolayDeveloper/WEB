using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DicRouteStagePerformers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "DicRouteStagePerformers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    RouteStageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicRouteStagePerformers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicRouteStagePerformers_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStagePerformers_DicPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "DicPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStagePerformers_DicRouteStages_RouteStageId",
                        column: x => x.RouteStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStagePerformers_DepartmentId",
                table: "DicRouteStagePerformers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStagePerformers_PositionId",
                table: "DicRouteStagePerformers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStagePerformers_RouteStageId",
                table: "DicRouteStagePerformers",
                column: "RouteStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicRouteStagePerformers");
        }
    }
}
