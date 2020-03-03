using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AttachmentProtectionDocRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProtectionDocId",
                table: "Attachments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ProtectionDocId",
                table: "Attachments",
                column: "ProtectionDocId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_ProtectionDocs_ProtectionDocId",
                table: "Attachments",
                column: "ProtectionDocId",
                principalTable: "ProtectionDocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_ProtectionDocs_ProtectionDocId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ProtectionDocId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ProtectionDocId",
                table: "Attachments");
        }
    }
}
