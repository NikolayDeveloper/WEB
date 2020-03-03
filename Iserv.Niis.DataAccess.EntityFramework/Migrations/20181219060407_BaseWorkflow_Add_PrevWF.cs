using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class BaseWorkflow_Add_PrevWF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //DocumentWorkflow
            migrationBuilder.AddColumn<int>(
                name: "PreviousWorkflowId",
                table: "DocumentWorkflows",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflows_PreviousWorkflowId",
                table: "DocumentWorkflows",
                column: "PreviousWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentWorkflows_PreviousWorkflowId",
                table: "DocumentWorkflows",
                column: "PreviousWorkflowId",
                principalTable: "DocumentWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //ProtectionDocWorkflow
            migrationBuilder.AddColumn<int>(
                name: "PreviousWorkflowId",
                table: "ProtectionDocWorkflows",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_PreviousWorkflowId",
                table: "ProtectionDocWorkflows",
                column: "PreviousWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtectionDocWorkflows_PreviousWorkflowId",
                table: "ProtectionDocWorkflows",
                column: "PreviousWorkflowId",
                principalTable: "ProtectionDocWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //ContractWorkflow
            migrationBuilder.AddColumn<int>(
               name: "PreviousWorkflowId",
               table: "ContractWorkflows",
               type: "int",
               nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractWorkflows_PreviousWorkflowId",
                table: "ContractWorkflows",
                column: "PreviousWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractWorkflows_PreviousWorkflowId",
                table: "ContractWorkflows",
                column: "PreviousWorkflowId",
                principalTable: "ContractWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            //RequestWorkflow
            migrationBuilder.AddColumn<int>(
                name: "PreviousWorkflowId",
                table: "RequestWorkflows",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_PreviousWorkflowId",
                table: "RequestWorkflows",
                column: "PreviousWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestWorkflows_PreviousWorkflowId",
                table: "RequestWorkflows",
                column: "PreviousWorkflowId",
                principalTable: "RequestWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //DocumentWorkflow
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentWorkflows_PreviousWorkflowId",
                table: "DocumentWorkflows");
            
            migrationBuilder.DropIndex(
                name: "IX_DocumentWorkflows_PreviousWorkflowId",
                table: "DocumentWorkflows");
            
            migrationBuilder.DropColumn(
                name: "PreviousWorkflowId",
                table: "DocumentWorkflows");

            //ProtectionDocWorkflow
            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocWorkflows_PreviousWorkflowId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_ProtectionDocWorkflows_PreviousWorkflowId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropColumn(
                name: "PreviousWorkflowId",
                table: "ProtectionDocWorkflows");

            //ContractWorkflow
            migrationBuilder.DropForeignKey(
                name: "FK_ContractWorkflows_PreviousWorkflowId",
                table: "ContractWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_ContractWorkflows_PreviousWorkflowId",
                table: "ContractWorkflows");

            migrationBuilder.DropColumn(
                name: "PreviousWorkflowId",
                table: "ContractWorkflows");

            //RequestWorkflow
            migrationBuilder.DropForeignKey(
                name: "FK_RequestWorkflows_PreviousWorkflowId",
                table: "RequestWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_RequestWorkflows_PreviousWorkflowId",
                table: "RequestWorkflows");

            migrationBuilder.DropColumn(
                name: "PreviousWorkflowId",
                table: "RequestWorkflows");
        }
    }
}
