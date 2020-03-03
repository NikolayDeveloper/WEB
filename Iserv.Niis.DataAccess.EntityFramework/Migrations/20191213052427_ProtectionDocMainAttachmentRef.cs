using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class ProtectionDocMainAttachmentRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainAttachmentId",
                table: "ProtectionDocs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_MainAttachmentId",
                table: "ProtectionDocs",
                column: "MainAttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtectionDocs_Attachments_MainAttachmentId",
                table: "ProtectionDocs",
                column: "MainAttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocs_Attachments_MainAttachmentId",
                table: "ProtectionDocs");

            migrationBuilder.DropIndex(
                name: "IX_ProtectionDocs_MainAttachmentId",
                table: "ProtectionDocs");

            migrationBuilder.DropColumn(
                name: "MainAttachmentId",
                table: "ProtectionDocs");
        }
    }
}
