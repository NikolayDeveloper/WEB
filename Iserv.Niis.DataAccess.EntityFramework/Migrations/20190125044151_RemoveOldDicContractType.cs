using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RemoveOldDicContractType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicContractTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DicContractTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassificationId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    ConServiceTypeCode = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    Interval = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsRequireSigning = table.Column<bool>(nullable: true),
                    IsSendByEmail = table.Column<bool>(nullable: true),
                    IsUnique = table.Column<bool>(nullable: true),
                    NameEn = table.Column<string>(maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(maxLength: 2000, nullable: true),
                    Order = table.Column<int>(nullable: true),
                    RouteId = table.Column<int>(nullable: true),
                    TemplateFileId = table.Column<int>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicContractTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicContractTypes_DicDocumentClassifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "DicDocumentClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicContractTypes_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicContractTypes_DocumentTemplateFiles_TemplateFileId",
                        column: x => x.TemplateFileId,
                        principalTable: "DocumentTemplateFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicContractTypes_ClassificationId",
                table: "DicContractTypes",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_DicContractTypes_RouteId",
                table: "DicContractTypes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_DicContractTypes_TemplateFileId",
                table: "DicContractTypes",
                column: "TemplateFileId");
        }
    }
}
