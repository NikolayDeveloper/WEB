using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddRequestConsiderationTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isFromLk",
                table: "Requests",
                newName: "IsFromLk");

            migrationBuilder.AddColumn<int>(
                name: "ConsiderationTypeId",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ConsiderationTypeId",
                table: "Requests",
                column: "ConsiderationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_DicConsiderationTypes_ConsiderationTypeId",
                table: "Requests",
                column: "ConsiderationTypeId",
                principalTable: "DicConsiderationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_DicConsiderationTypes_ConsiderationTypeId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ConsiderationTypeId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ConsiderationTypeId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "IsFromLk",
                table: "Requests",
                newName: "isFromLk");
        }
    }
}
