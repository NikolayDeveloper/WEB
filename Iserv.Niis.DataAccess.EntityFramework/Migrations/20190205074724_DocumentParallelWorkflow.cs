using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DocumentParallelWorkflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.AddColumn<bool>(
                name: "IsCurent",
                table: "DocumentWorkflows",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"UPDATE dbo.DocumentWorkflows SET IsCurent = 1 WHERE EXISTS(SELECT * FROM dbo.Documents WHERE dbo.[Documents].CurrentWorkflowId = dbo.[DocumentWorkflows].Id)");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentWorkflows_CurrentWorkflowId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_CurrentWorkflowId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CurrentWorkflowId",
                table: "Documents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurent",
                table: "DocumentWorkflows");

            migrationBuilder.AddColumn<int>(
                name: "CurrentWorkflowId",
                table: "Documents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CurrentWorkflowId",
                table: "Documents",
                column: "CurrentWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentWorkflows_CurrentWorkflowId",
                table: "Documents",
                column: "CurrentWorkflowId",
                principalTable: "DocumentWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
