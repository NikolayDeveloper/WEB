using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DicTariffProtectionDocType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiveTypeGroup",
                table: "DicTariffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "DicReceiveTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DicTariffProtectionDocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: false),
                    TariffId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicTariffProtectionDocTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicTariffProtectionDocTypes_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicTariffProtectionDocTypes_DicTariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "DicTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicTariffProtectionDocTypes_ProtectionDocTypeId",
                table: "DicTariffProtectionDocTypes",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicTariffProtectionDocTypes_TariffId",
                table: "DicTariffProtectionDocTypes",
                column: "TariffId");

            migrationBuilder.Sql(@"INSERT INTO dbo.DicTariffProtectionDocTypes(TariffId, ProtectionDocTypeId, DateCreate, DateUpdate, IsDeleted)
                                   SELECT dt.Id, dt.ProtectionDocTypeId, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0 FROM dbo.DicTariffs dt WHERE ProtectionDocTypeId IS NOT NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_DicTariffs_DicProtectionDocTypes_ProtectionDocTypeId",
                table: "DicTariffs");

            migrationBuilder.DropForeignKey(
                name: "FK_DicTariffs_DicReceiveTypes_ReceiveTypeId",
                table: "DicTariffs");

            migrationBuilder.DropIndex(
                name: "IX_DicTariffs_ProtectionDocTypeId",
                table: "DicTariffs");

            migrationBuilder.DropIndex(
                name: "IX_DicTariffs_ReceiveTypeId",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "ProtectionDocTypeId",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "ReceiveTypeId",
                table: "DicTariffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicTariffProtectionDocTypes");

            migrationBuilder.DropColumn(
                name: "ReceiveTypeGroup",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "DicReceiveTypes");

            migrationBuilder.AddColumn<int>(
                name: "ProtectionDocTypeId",
                table: "DicTariffs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiveTypeId",
                table: "DicTariffs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DicTariffs_ProtectionDocTypeId",
                table: "DicTariffs",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicTariffs_ReceiveTypeId",
                table: "DicTariffs",
                column: "ReceiveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DicTariffs_DicProtectionDocTypes_ProtectionDocTypeId",
                table: "DicTariffs",
                column: "ProtectionDocTypeId",
                principalTable: "DicProtectionDocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DicTariffs_DicReceiveTypes_ReceiveTypeId",
                table: "DicTariffs",
                column: "ReceiveTypeId",
                principalTable: "DicReceiveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
