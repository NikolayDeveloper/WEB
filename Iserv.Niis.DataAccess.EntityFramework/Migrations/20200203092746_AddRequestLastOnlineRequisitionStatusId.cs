using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddRequestLastOnlineRequisitionStatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastOnlineRequisitionStatusId",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_LastOnlineRequisitionStatusId",
                table: "Requests",
                column: "LastOnlineRequisitionStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_DicOnlineRequisitionStatuses_LastOnlineRequisitionStatusId",
                table: "Requests",
                column: "LastOnlineRequisitionStatusId",
                principalTable: "DicOnlineRequisitionStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_DicOnlineRequisitionStatuses_LastOnlineRequisitionStatusId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_LastOnlineRequisitionStatusId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "LastOnlineRequisitionStatusId",
                table: "Requests");
        }
    }
}
