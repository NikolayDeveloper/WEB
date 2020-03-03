using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class CreateNotificationAutoGenerationRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationDocumentByPetitionAndPaymentRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    PetitionTypeId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    TariffId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDocumentByPetitionAndPaymentRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationDocumentByPetitionAndPaymentRules_DicDocumentTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationDocumentByPetitionAndPaymentRules_DicDocumentTypes_PetitionTypeId",
                        column: x => x.PetitionTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationDocumentByPetitionAndPaymentRules_DicRouteStages_StageId",
                        column: x => x.StageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationDocumentByPetitionAndPaymentRules_DicTariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "DicTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationDocumentByStageRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDocumentByStageRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationDocumentByStageRules_DicDocumentTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationDocumentByStageRules_DicRouteStages_StageId",
                        column: x => x.StageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDocumentByPetitionAndPaymentRules_NotificationTypeId",
                table: "NotificationDocumentByPetitionAndPaymentRules",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDocumentByPetitionAndPaymentRules_PetitionTypeId",
                table: "NotificationDocumentByPetitionAndPaymentRules",
                column: "PetitionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDocumentByPetitionAndPaymentRules_StageId",
                table: "NotificationDocumentByPetitionAndPaymentRules",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDocumentByPetitionAndPaymentRules_TariffId",
                table: "NotificationDocumentByPetitionAndPaymentRules",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDocumentByStageRules_NotificationTypeId",
                table: "NotificationDocumentByStageRules",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDocumentByStageRules_StageId",
                table: "NotificationDocumentByStageRules",
                column: "StageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationDocumentByPetitionAndPaymentRules");

            migrationBuilder.DropTable(
                name: "NotificationDocumentByStageRules");
        }
    }
}
