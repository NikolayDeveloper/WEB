using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class NewDicContractType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DicContractTypes",
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
                    StageOneId = table.Column<int>(type: "int", nullable: false),
                    StageTwoId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicContractTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicContractTypes_DicCustomerRoles_StageOneId",
                        column: x => x.StageOneId,
                        principalTable: "DicCustomerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicContractTypes_DicCustomerRoles_StageTwoId",
                        column: x => x.StageTwoId,
                        principalTable: "DicCustomerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicContractTypes_StageOneId",
                table: "DicContractTypes",
                column: "StageOneId");

            migrationBuilder.CreateIndex(
                name: "IX_DicContractTypes_StageTwoId",
                table: "DicContractTypes",
                column: "StageTwoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicContractTypes");
        }
    }
}
