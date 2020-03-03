using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class AddDicDocumentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DicDocumentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDocumentStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StatusId",
                table: "Documents",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DicDocumentStatuses_StatusId",
                table: "Documents",
                column: "StatusId",
                principalTable: "DicDocumentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(@"insert into [DicDocumentStatuses] ([Code], [DateCreate], [DateUpdate], [NameEn], [NameKz], [NameRu]) 
                                   values(1, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'In Work', N'В работе', N'В работе'),
                                         (2, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Completed', N'Завершён', N'Завершён')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DicDocumentStatuses_StatusId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "DicDocumentStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Documents_StatusId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Documents");
        }
    }
}
