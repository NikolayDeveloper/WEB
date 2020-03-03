using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class UpdateDicRouteStagePerformers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DicRouteStagePerformers_DicDepartments_DepartmentId",
                table: "DicRouteStagePerformers");

            migrationBuilder.DropForeignKey(
                name: "FK_DicRouteStagePerformers_DicPositions_PositionId",
                table: "DicRouteStagePerformers");

            migrationBuilder.DropIndex(
                name: "IX_DicRouteStagePerformers_DepartmentId",
                table: "DicRouteStagePerformers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "DicRouteStagePerformers");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "DicRouteStagePerformers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DicRouteStagePerformers_PositionId",
                table: "DicRouteStagePerformers",
                newName: "IX_DicRouteStagePerformers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DicRouteStagePerformers_AspNetUsers_UserId",
                table: "DicRouteStagePerformers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DicRouteStagePerformers_AspNetUsers_UserId",
                table: "DicRouteStagePerformers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DicRouteStagePerformers",
                newName: "PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_DicRouteStagePerformers_UserId",
                table: "DicRouteStagePerformers",
                newName: "IX_DicRouteStagePerformers_PositionId");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "DicRouteStagePerformers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStagePerformers_DepartmentId",
                table: "DicRouteStagePerformers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DicRouteStagePerformers_DicDepartments_DepartmentId",
                table: "DicRouteStagePerformers",
                column: "DepartmentId",
                principalTable: "DicDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DicRouteStagePerformers_DicPositions_PositionId",
                table: "DicRouteStagePerformers",
                column: "PositionId",
                principalTable: "DicPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
