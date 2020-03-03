using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class PaymentInvoiceDocIdRefFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInvoices_Documents_DocumentId",
                table: "PaymentInvoices");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInvoices_DocumentId",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "PaymentInvoices");

            migrationBuilder.DropColumn(
                name: "PaymentService",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "PaymentInvoiceId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PaymentInvoiceId",
                table: "Documents",
                column: "PaymentInvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_PaymentInvoices_PaymentInvoiceId",
                table: "Documents",
                column: "PaymentInvoiceId",
                principalTable: "PaymentInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_PaymentInvoices_PaymentInvoiceId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PaymentInvoiceId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "PaymentInvoiceId",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "PaymentInvoices",
                nullable: true);
            
            migrationBuilder.AddColumn<string>(
                name: "PaymentService",
                table: "Documents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_DocumentId",
                table: "PaymentInvoices",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoices_Documents_DocumentId",
                table: "PaymentInvoices",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
