using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RemoveProtectionDocAttorney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtectionDocAttorneys");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProtectionDocAttorneys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    CertificateDate = table.Column<DateTimeOffset>(nullable: false),
                    CertificateNumber = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Job = table.Column<string>(nullable: true),
                    KnowledgeArea = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    NameFirst = table.Column<string>(nullable: true),
                    NameLast = table.Column<string>(nullable: true),
                    NameMiddle = table.Column<string>(nullable: true),
                    OPS = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    RevalidNote = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    XIN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocAttorneys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocAttorneys_DicCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocAttorneys_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocAttorneys_DicLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "DicLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocAttorneys_CountryId",
                table: "ProtectionDocAttorneys",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocAttorneys_CustomerId",
                table: "ProtectionDocAttorneys",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocAttorneys_LocationId",
                table: "ProtectionDocAttorneys",
                column: "LocationId");
        }
    }
}
