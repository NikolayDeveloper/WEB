using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetClaimConstants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetClaimConstants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bulletin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bulletin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicApplicantTypes",
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
                    table.PrimaryKey("PK_DicApplicantTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicBeneficiaryTypes",
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
                    table.PrimaryKey("PK_DicBeneficiaryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicColorTZs",
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
                    table.PrimaryKey("PK_DicColorTZs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicContactInfoTypes",
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
                    table.PrimaryKey("PK_DicContactInfoTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicContinents",
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
                    Order = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicContinents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicContinents_DicContinents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DicContinents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicContractCategories",
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
                    table.PrimaryKey("PK_DicContractCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicContractStatuses",
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
                    table.PrimaryKey("PK_DicContractStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicConventionTypes",
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
                    table.PrimaryKey("PK_DicConventionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicCustomerRoles",
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
                    table.PrimaryKey("PK_DicCustomerRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicCustomerTypes",
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
                    table.PrimaryKey("PK_DicCustomerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicDepartmentTypes",
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
                    table.PrimaryKey("PK_DicDepartmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicDivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IncomingNumberCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMonitoring = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDivisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicDocumentClassifications",
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
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDocumentClassifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicDocumentClassifications_DicDocumentClassifications_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DicDocumentClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicEventTypes",
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
                    table.PrimaryKey("PK_DicEventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicICFEMs",
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
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicICFEMs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicICFEMs_DicICFEMs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DicICFEMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicICGSs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionShort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RevisionNumber = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicICGSs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicICISs",
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
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    RevisionNumber = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicICISs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicICISs_DicICISs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DicICISs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicIntellectualPropertyStatuses",
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
                    table.PrimaryKey("PK_DicIntellectualPropertyStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicIPCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Kind = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    RevisionNumber = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicIPCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicIPCs_DicIPCs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DicIPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicLocationTypes",
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
                    table.PrimaryKey("PK_DicLocationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicLogTypes",
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
                    table.PrimaryKey("PK_DicLogTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicNotificationStatuses",
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
                    table.PrimaryKey("PK_DicNotificationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicOnlineRequisitionStatuses",
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
                    table.PrimaryKey("PK_DicOnlineRequisitionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicPaymentStatuses",
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
                    table.PrimaryKey("PK_DicPaymentStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicProtectionDocBulletinTypes",
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
                    table.PrimaryKey("PK_DicProtectionDocBulletinTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicProtectionDocStatuses",
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
                    table.PrimaryKey("PK_DicProtectionDocStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicProtectionDocTMTypes",
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
                    table.PrimaryKey("PK_DicProtectionDocTMTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicReceiveTypes",
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
                    table.PrimaryKey("PK_DicReceiveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicRedefinitionDocumentTypes",
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
                    table.PrimaryKey("PK_DicRedefinitionDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicRedefinitionTypes",
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
                    table.PrimaryKey("PK_DicRedefinitionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicRequestStatuses",
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
                    table.PrimaryKey("PK_DicRequestStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicRoutes",
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
                    table.PrimaryKey("PK_DicRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicSelectionAchieveTypes",
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
                    table.PrimaryKey("PK_DicSelectionAchieveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicSendTypes",
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
                    table.PrimaryKey("PK_DicSendTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DicTypeTrademarks",
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
                    table.PrimaryKey("PK_DicTypeTrademarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplateFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileFingerPrint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplateFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationConPackageStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationConPackageStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationConPackageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationConPackageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationConServiceStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationConServiceStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETDATE()"),
                    DateSent = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DocumentBarcode = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InOutDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    InOutNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestBarcode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationEGovPays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PaySum = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PayXin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayXml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestBarcode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEGovPays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationLogRefLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SqLquery = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationLogRefLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationNiisRefTariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueBiz = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ValueFiz = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ValueFizBenefit = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ValueFull = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ValueJur = table.Column<decimal>(type: "decimal(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationNiisRefTariffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationPaymentCalcs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CorId = table.Column<int>(type: "int", nullable: false),
                    CountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinCount = table.Column<int>(type: "int", nullable: true),
                    PatentType = table.Column<int>(type: "int", nullable: false),
                    TariffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationPaymentCalcs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationRomarinFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationRomarinFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationRomarinLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "xml", nullable: true),
                    TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationRomarinLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateDataFileId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CountryNameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<byte>(type: "tinyint", nullable: true),
                    Num = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerType = table.Column<int>(type: "int", nullable: false),
                    ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
                    ReceiveTypeNameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchView", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemCounter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCounter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicCountries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContinentId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicCountries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicCountries_DicContinents_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "DicContinents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicCountries_DicCountries_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsMonitoring = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ShortNameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TNameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicDepartments_DicDepartmentTypes_DepartmentTypeId",
                        column: x => x.DepartmentTypeId,
                        principalTable: "DicDepartmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicDepartments_DicDivisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "DicDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_DicEventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "DicEventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicDetailICGSs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IcgsId = table.Column<int>(type: "int", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameFr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDetailICGSs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicDetailICGSs_DicICGSs_IcgsId",
                        column: x => x.IcgsId,
                        principalTable: "DicICGSs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETDATE()"),
                    DateSent = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnlineRequisitionStatusId = table.Column<int>(type: "int", nullable: false),
                    RequestBarcode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationStatuses_DicOnlineRequisitionStatuses_OnlineRequisitionStatusId",
                        column: x => x.OnlineRequisitionStatusId,
                        principalTable: "DicOnlineRequisitionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentTicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentWeigth = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    SendAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SendTypeId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentProperties_DicSendTypes_SendTypeId",
                        column: x => x.SendTypeId,
                        principalTable: "DicSendTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicContractTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassificationId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConServiceTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Interval = table.Column<int>(type: "int", nullable: true),
                    IsRequireSigning = table.Column<bool>(type: "bit", nullable: true),
                    IsSendByEmail = table.Column<bool>(type: "bit", nullable: true),
                    IsUnique = table.Column<bool>(type: "bit", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    TemplateFileId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "DicDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClassificationId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConServiceTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Interval = table.Column<int>(type: "int", nullable: true),
                    IsRequireSigning = table.Column<bool>(type: "bit", nullable: true),
                    IsSendByEmail = table.Column<bool>(type: "bit", nullable: true),
                    IsUnique = table.Column<bool>(type: "bit", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    TemplateFileId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDocumentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicDocumentTypes_DicDocumentClassifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "DicDocumentClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicDocumentTypes_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicDocumentTypes_DocumentTemplateFiles_TemplateFileId",
                        column: x => x.TemplateFileId,
                        principalTable: "DocumentTemplateFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationConPackages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateProcess = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    PackageData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationConPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationConPackages_IntegrationConPackageStates_StateId",
                        column: x => x.StateId,
                        principalTable: "IntegrationConPackageStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationConPackages_IntegrationConPackageTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "IntegrationConPackageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicRouteStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractStatusId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationType = table.Column<int>(type: "int", nullable: false),
                    ExpirationValue = table.Column<short>(type: "smallint", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FinishConServiceStatusId = table.Column<int>(type: "int", nullable: true),
                    Interval = table.Column<int>(type: "int", nullable: true),
                    IsAuto = table.Column<bool>(type: "bit", nullable: false),
                    IsFirst = table.Column<bool>(type: "bit", nullable: false),
                    IsLast = table.Column<bool>(type: "bit", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    IsMultiUser = table.Column<bool>(type: "bit", nullable: false),
                    IsReturnable = table.Column<bool>(type: "bit", nullable: true),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OnlineRequisitionStatusId = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocStatusId = table.Column<int>(type: "int", nullable: true),
                    RequestStatusId = table.Column<int>(type: "int", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    StartConServiceStatusId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicRouteStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicRouteStages_DicContractStatuses_ContractStatusId",
                        column: x => x.ContractStatusId,
                        principalTable: "DicContractStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStages_IntegrationConServiceStatuses_FinishConServiceStatusId",
                        column: x => x.FinishConServiceStatusId,
                        principalTable: "IntegrationConServiceStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStages_DicOnlineRequisitionStatuses_OnlineRequisitionStatusId",
                        column: x => x.OnlineRequisitionStatusId,
                        principalTable: "DicOnlineRequisitionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStages_DicProtectionDocStatuses_ProtectionDocStatusId",
                        column: x => x.ProtectionDocStatusId,
                        principalTable: "DicProtectionDocStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStages_DicRequestStatuses_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "DicRequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStages_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicRouteStages_IntegrationConServiceStatuses_StartConServiceStatusId",
                        column: x => x.StartConServiceStatusId,
                        principalTable: "IntegrationConServiceStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    Apartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantsInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeneficiaryTypeId = table.Column<int>(type: "int", nullable: true),
                    CertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateSeries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsBeneficiary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsNotMention = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsNotResident = table.Column<bool>(type: "bit", nullable: false),
                    IsSMB = table.Column<bool>(type: "bit", nullable: true),
                    JurRegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameEnLong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKzLong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRuLong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotaryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneFax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerAttorneyDateIssue = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PowerAttorneyFullNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Rnn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDocContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subscript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Xin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicCustomers_DicBeneficiaryTypes_BeneficiaryTypeId",
                        column: x => x.BeneficiaryTypeId,
                        principalTable: "DicBeneficiaryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicCustomers_DicCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicCustomers_DicCustomerTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicCustomerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    StatId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicLocations_DicCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicLocations_DicLocations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DicLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicLocations_DicLocationTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicLocationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsHead = table.Column<bool>(type: "bit", nullable: true),
                    IsMonitoring = table.Column<bool>(type: "bit", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicPositions_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicProtectionDocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DepatmentId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DkCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocTypeText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocTypeTextKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicProtectionDocTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicProtectionDocTypes_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicProtectionDocTypes_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role_RouteStage",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_RouteStage", x => new { x.RoleId, x.StageId });
                    table.ForeignKey(
                        name: "FK_Role_RouteStage_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_RouteStage_DicRouteStages_StageId",
                        column: x => x.StageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RouteStageOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentStageId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false),
                    IsParallel = table.Column<bool>(type: "bit", nullable: false),
                    IsReturn = table.Column<bool>(type: "bit", nullable: false),
                    NextStageId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStageOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteStageOrders_DicRouteStages_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteStageOrders_DicRouteStages_NextStageId",
                        column: x => x.NextStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DicCustomerId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfos_DicCustomers_DicCustomerId",
                        column: x => x.DicCustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactInfos_DicContactInfoTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicContactInfoTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAttorneyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateBeginStop = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCard = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateDisCard = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateEndStop = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DatePublic = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FieldOfActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldOfKnowledge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovReg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovRegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicRedefine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Redefine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SomeDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    WorkPlace = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAttorneyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAttorneyInfos_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    AssignmentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CurrencyRate = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CurrencyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsPrePayment = table.Column<bool>(type: "bit", nullable: true),
                    Payment1CNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurposeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContinentId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicAddresses_DicContinents_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "DicContinents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicAddresses_DicCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicAddresses_DicLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "DicLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocAttorneys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KnowledgeArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    NameFirst = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameLast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameMiddle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OPS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevalidNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XIN = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultyPriority = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchive = table.Column<bool>(type: "bit", nullable: true),
                    IsVirtual = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MaximumLoad = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    XIN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_DicPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "DicPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AvailabilityCorrespondences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: false),
                    RouteStageId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityCorrespondences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailabilityCorrespondences_DicDocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AvailabilityCorrespondences_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AvailabilityCorrespondences_DicRouteStages_RouteStageId",
                        column: x => x.RouteStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicConsiderationTypes",
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
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicConsiderationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicConsiderationTypes_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicDocumentTypesDicProtectionDocTypes",
                columns: table => new
                {
                    DicDocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DicProtectionDocTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicDocumentTypesDicProtectionDocTypes", x => new { x.DicDocumentTypeId, x.DicProtectionDocTypeId });
                    table.ForeignKey(
                        name: "FK_DicDocumentTypesDicProtectionDocTypes_DicDocumentTypes_DicDocumentTypeId",
                        column: x => x.DicDocumentTypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicDocumentTypesDicProtectionDocTypes_DicProtectionDocTypes_DicProtectionDocTypeId",
                        column: x => x.DicProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicEarlyRegTypes",
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
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicEarlyRegTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicEarlyRegTypes_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicProtectionDocSubTypes",
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
                    S1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    S1Kz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    S2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    S2Kz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicProtectionDocSubTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicProtectionDocSubTypes_DicProtectionDocTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicRequisitionFeedTypes",
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
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicRequisitionFeedTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicRequisitionFeedTypes_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicTariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsProtectionDocSupportDateExpired = table.Column<bool>(type: "bit", nullable: true),
                    Limit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintenanceYears = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NiisTariffId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PriceBeneficiary = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PriceBusiness = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PriceFl = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PriceUl = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ProtectionDocSupportYearsFrom = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocSupportYearsUntil = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
                    ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicTariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicTariffs_IntegrationNiisRefTariffs_NiisTariffId",
                        column: x => x.NiisTariffId,
                        principalTable: "IntegrationNiisRefTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicTariffs_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicTariffs_DicReceiveTypes_ReceiveTypeId",
                        column: x => x.ReceiveTypeId,
                        principalTable: "DicReceiveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationRequisitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Callback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChainId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETDATE()"),
                    IsCallbackProcessed = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnlineRequisitionStatusId = table.Column<int>(type: "int", nullable: false),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: false),
                    RequestBarcode = table.Column<int>(type: "int", nullable: false),
                    RequestNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationRequisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationRequisitions_DicOnlineRequisitionStatuses_OnlineRequisitionStatusId",
                        column: x => x.OnlineRequisitionStatusId,
                        principalTable: "DicOnlineRequisitionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationRequisitions_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role_ProtectionDocType",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_ProtectionDocType", x => new { x.RoleId, x.ProtectionDocTypeId });
                    table.ForeignKey(
                        name: "FK_Role_ProtectionDocType_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_ProtectionDocType_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserICGSs",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IcgsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserICGSs", x => new { x.UserId, x.IcgsId });
                    table.ForeignKey(
                        name: "FK_AspNetUserICGSs_DicICGSs_IcgsId",
                        column: x => x.IcgsId,
                        principalTable: "DicICGSs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserICGSs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserIpcs",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IpcId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserIpcs", x => new { x.UserId, x.IpcId });
                    table.ForeignKey(
                        name: "FK_AspNetUserIpcs_DicIPCs_IpcId",
                        column: x => x.IpcId,
                        principalTable: "DicIPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserIpcs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAccessRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true),
                    ClassificationId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAccessRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentAccessRoles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentAccessRoles_DicDocumentClassifications_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "DicDocumentClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentAccessRoles_DicDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GridPrintSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    GridName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintFields = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateDataFileId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridPrintSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridPrintSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SettingGridOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    GridName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingGridOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingGridOptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Signatures",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileFingerPrint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signatures", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Signatures_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_RouteStage",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_RouteStage", x => new { x.UserId, x.StageId });
                    table.ForeignKey(
                        name: "FK_User_RouteStage_DicRouteStages_StageId",
                        column: x => x.StageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_RouteStage_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicEntityAccessTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentAccessPermissionsId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicEntityAccessTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DicEntityAccessTypes_DocumentAccessRoles_DocumentAccessPermissionsId",
                        column: x => x.DocumentAccessPermissionsId,
                        principalTable: "DocumentAccessRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    BucketName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyCount = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    ValidName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddresseeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddresseeId = table.Column<int>(type: "int", nullable: true),
                    ApplicantTypeId = table.Column<int>(type: "int", nullable: true),
                    ApplicationNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<int>(type: "int", nullable: false),
                    BulletinDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractTypeId = table.Column<int>(type: "int", nullable: true),
                    CopyCount = table.Column<int>(type: "int", nullable: true),
                    CurrentWorkflowId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DivisionId = table.Column<int>(type: "int", nullable: true),
                    ExtensionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FullExpertiseExecutorId = table.Column<int>(type: "int", nullable: true),
                    GosDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    GosNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    MainAttachmentId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberBulletin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutgoingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OutgoingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    PaperworkStateRegister = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: false),
                    ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegistrationPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    TerminateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    ValidDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_DicCustomers_AddresseeId",
                        column: x => x.AddresseeId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicApplicantTypes_ApplicantTypeId",
                        column: x => x.ApplicantTypeId,
                        principalTable: "DicApplicantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicContractCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DicContractCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicProtectionDocSubTypes_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "DicProtectionDocSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicDivisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "DicDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_FullExpertiseExecutorId",
                        column: x => x.FullExpertiseExecutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Attachments_MainAttachmentId",
                        column: x => x.MainAttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicReceiveTypes_ReceiveTypeId",
                        column: x => x.ReceiveTypeId,
                        principalTable: "DicReceiveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_DicContractStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DicContractStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerRoleId = table.Column<int>(type: "int", nullable: true),
                    DateBegin = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneFax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractCustomers_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractCustomers_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractCustomers_DicCustomerRoles_CustomerRoleId",
                        column: x => x.CustomerRoleId,
                        principalTable: "DicCustomerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractsNotificationStatuses",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    NotificationStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractsNotificationStatuses", x => new { x.ContractId, x.NotificationStatusId });
                    table.ForeignKey(
                        name: "FK_ContractsNotificationStatuses_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractsNotificationStatuses_DicNotificationStatuses_NotificationStatusId",
                        column: x => x.NotificationStatusId,
                        principalTable: "DicNotificationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ControlDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CurrentStageId = table.Column<int>(type: "int", nullable: true),
                    CurrentUserId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateReceived = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FromStageId = table.Column<int>(type: "int", nullable: true),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractWorkflows_DicRouteStages_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractWorkflows_AspNetUsers_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractWorkflows_DicRouteStages_FromStageId",
                        column: x => x.FromStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractWorkflows_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractWorkflows_Contracts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractWorkflows_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentExecutors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentExecutors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentExecutors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentUserSignatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsValidCertificate = table.Column<bool>(type: "bit", nullable: false),
                    PlainData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignerCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkflowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUserSignatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentUserSignatures_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ControlDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CurrentStageId = table.Column<int>(type: "int", nullable: true),
                    CurrentUserId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateReceived = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FromStageId = table.Column<int>(type: "int", nullable: true),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentWorkflows_DicRouteStages_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentWorkflows_AspNetUsers_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentWorkflows_DicRouteStages_FromStageId",
                        column: x => x.FromStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentWorkflows_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentWorkflows_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddresseeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddresseeId = table.Column<int>(type: "int", nullable: true),
                    Barcode = table.Column<int>(type: "int", nullable: false),
                    BulletinId = table.Column<int>(type: "int", nullable: true),
                    CurrentWorkflowId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DivisionId = table.Column<int>(type: "int", nullable: true),
                    DocumentNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<byte>(type: "tinyint", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IncomingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncomingNumberFilial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: true),
                    MainAttachmentId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutgoingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
                    ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
                    SendingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_DicCustomers_AddresseeId",
                        column: x => x.AddresseeId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Bulletin_BulletinId",
                        column: x => x.BulletinId,
                        principalTable: "Bulletin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentWorkflows_CurrentWorkflowId",
                        column: x => x.CurrentWorkflowId,
                        principalTable: "DocumentWorkflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DicDivisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "DicDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Attachments_MainAttachmentId",
                        column: x => x.MainAttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DicReceiveTypes_ReceiveTypeId",
                        column: x => x.ReceiveTypeId,
                        principalTable: "DicReceiveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_DicDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContractsDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractsDocuments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContractsDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Document_Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerRoleId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsMention = table.Column<bool>(type: "bit", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Customer_DicAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "DicAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Document_Customer_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Document_Customer_DicCustomerRoles_CustomerRoleId",
                        column: x => x.CustomerRoleId,
                        principalTable: "DicCustomerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Document_Customer_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentContents_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDocumentRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsAnswer = table.Column<bool>(type: "bit", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDocumentRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentDocumentRelations_Documents_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentDocumentRelations_Documents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentEarlyRegs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    EarlyRegTypeId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameSD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StageSD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentEarlyRegs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentEarlyRegs_DicCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentEarlyRegs_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentEarlyRegs_DicEarlyRegTypes_EarlyRegTypeId",
                        column: x => x.EarlyRegTypeId,
                        principalTable: "DicEarlyRegTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentsNotificationStatuses",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    NotificationStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsNotificationStatuses", x => new { x.DocumentId, x.NotificationStatusId });
                    table.ForeignKey(
                        name: "FK_DocumentsNotificationStatuses_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentsNotificationStatuses_DicNotificationStatuses_NotificationStatusId",
                        column: x => x.NotificationStatusId,
                        principalTable: "DicNotificationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentUserInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserInput = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUserInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentUserInputs_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedInvoiceNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Index = table.Column<int>(type: "int", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedInvoiceNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedInvoiceNumbers_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneratedInvoiceNumbers_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneratedInvoiceNumbers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedQueryExpDeps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedQueryExpDeps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedQueryExpDeps_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneratedQueryExpDeps_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicantTypeId = table.Column<int>(type: "int", nullable: true),
                    Coefficient = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    CreateUserId = table.Column<int>(type: "int", nullable: true),
                    DateComplete = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateFact = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true),
                    Nds = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    OverdueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PenaltyPercent = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TariffCount = table.Column<int>(type: "int", nullable: true),
                    TariffId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    WhoBoundUserId = table.Column<int>(type: "int", nullable: true),
                    WriteOffUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentInvoices_DicApplicantTypes_ApplicantTypeId",
                        column: x => x.ApplicantTypeId,
                        principalTable: "DicApplicantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoices_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoices_AspNetUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoices_DicPaymentStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DicPaymentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoices_DicTariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "DicTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoices_AspNetUsers_WhoBoundUserId",
                        column: x => x.WhoBoundUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentInvoices_AspNetUsers_WriteOffUserId",
                        column: x => x.WriteOffUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRegistryDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    PaymentInvoiceId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRegistryDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRegistryDatas_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentRegistryDatas_PaymentInvoices_PaymentInvoiceId",
                        column: x => x.PaymentInvoiceId,
                        principalTable: "PaymentInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentUses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CreateUserId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    PaymentInvoiceId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentUses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentUses_AspNetUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentUses_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentUses_PaymentInvoices_PaymentInvoiceId",
                        column: x => x.PaymentInvoiceId,
                        principalTable: "PaymentInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddresseeId = table.Column<int>(type: "int", nullable: true),
                    ApplicantTypeId = table.Column<int>(type: "int", nullable: true),
                    Barcode = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryTypeId = table.Column<int>(type: "int", nullable: true),
                    BulletinUserId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code60 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsiderationTypeId = table.Column<int>(type: "int", nullable: true),
                    ConventionTypeId = table.Column<int>(type: "int", nullable: true),
                    CopyrightAuthor = table.Column<bool>(type: "bit", nullable: true),
                    CopyrightEmployer = table.Column<bool>(type: "bit", nullable: true),
                    CurrentWorkflowId = table.Column<int>(type: "int", nullable: true),
                    DataInitialPublication = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeclarantEmployer = table.Column<bool>(type: "bit", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclaimerKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclaimerRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarlyTerminationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExtensionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExtensionDateTz = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    GosDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    GosNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gosreestr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    IntellectualPropertyId = table.Column<int>(type: "int", nullable: true),
                    IsImageFromName = table.Column<bool>(type: "bit", nullable: false),
                    LicenseInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseInfoStateRegister = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintainDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberApxWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberCopyrightCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Otkaz = table.Column<bool>(type: "bit", nullable: true),
                    OutgoingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OutgoingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    PaperworkStateRegister = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviewImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProtectionDocInfoId = table.Column<int>(type: "int", nullable: true),
                    ProxyForDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProxyWithDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PublicDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RecoveryPetitionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Referat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    SelectionAchieveTypeId = table.Column<int>(type: "int", nullable: true),
                    SelectionFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectionNameOffer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendTypeId = table.Column<int>(type: "int", nullable: true),
                    SmallImageId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    SubTypeId = table.Column<int>(type: "int", nullable: true),
                    SupportUserId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    ToPm = table.Column<int>(type: "int", nullable: true),
                    TransferDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Transliteration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    TypeTrademarkId = table.Column<int>(type: "int", nullable: true),
                    ValidDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicCustomers_AddresseeId",
                        column: x => x.AddresseeId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicApplicantTypes_ApplicantTypeId",
                        column: x => x.ApplicantTypeId,
                        principalTable: "DicApplicantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicBeneficiaryTypes_BeneficiaryTypeId",
                        column: x => x.BeneficiaryTypeId,
                        principalTable: "DicBeneficiaryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_AspNetUsers_BulletinUserId",
                        column: x => x.BulletinUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicConsiderationTypes_ConsiderationTypeId",
                        column: x => x.ConsiderationTypeId,
                        principalTable: "DicConsiderationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicConventionTypes_ConventionTypeId",
                        column: x => x.ConventionTypeId,
                        principalTable: "DicConventionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicSelectionAchieveTypes_SelectionAchieveTypeId",
                        column: x => x.SelectionAchieveTypeId,
                        principalTable: "DicSelectionAchieveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicSendTypes_SendTypeId",
                        column: x => x.SendTypeId,
                        principalTable: "DicSendTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicProtectionDocStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DicProtectionDocStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicProtectionDocSubTypes_SubTypeId",
                        column: x => x.SubTypeId,
                        principalTable: "DicProtectionDocSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_AspNetUsers_SupportUserId",
                        column: x => x.SupportUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicProtectionDocTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocs_DicTypeTrademarks_TypeTrademarkId",
                        column: x => x.TypeTrademarkId,
                        principalTable: "DicTypeTrademarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract_ProtectionDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract_ProtectionDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_ProtectionDoc_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contract_ProtectionDoc_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicColorTZ_ProtectionDoc",
                columns: table => new
                {
                    ColorTzId = table.Column<int>(type: "int", nullable: false),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicColorTZ_ProtectionDoc", x => new { x.ColorTzId, x.ProtectionDocId });
                    table.ForeignKey(
                        name: "FK_DicColorTZ_ProtectionDoc_DicColorTZs_ColorTzId",
                        column: x => x.ColorTzId,
                        principalTable: "DicColorTZs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicColorTZ_ProtectionDoc_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicIcfem_ProtectionDoc",
                columns: table => new
                {
                    DicIcfemId = table.Column<int>(type: "int", nullable: false),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicIcfem_ProtectionDoc", x => new { x.DicIcfemId, x.ProtectionDocId });
                    table.ForeignKey(
                        name: "FK_DicIcfem_ProtectionDoc_DicICFEMs_DicIcfemId",
                        column: x => x.DicIcfemId,
                        principalTable: "DicICFEMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicIcfem_ProtectionDoc_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ICGS_ProtectionDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimedDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IcgsId = table.Column<int>(type: "int", nullable: false),
                    IsNegative = table.Column<bool>(type: "bit", nullable: true),
                    IsNegativePartial = table.Column<bool>(type: "bit", nullable: true),
                    NegativeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICGS_ProtectionDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ICGS_ProtectionDoc_DicICGSs_IcgsId",
                        column: x => x.IcgsId,
                        principalTable: "DicICGSs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ICGS_ProtectionDoc_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ICIS_ProtectionDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IcisId = table.Column<int>(type: "int", nullable: false),
                    ImportedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICIS_ProtectionDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ICIS_ProtectionDoc_DicICISs_IcisId",
                        column: x => x.IcisId,
                        principalTable: "DicICISs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ICIS_ProtectionDoc_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IPC_ProtectionDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IpcId = table.Column<int>(type: "int", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPC_ProtectionDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IPC_ProtectionDoc_DicIPCs_IpcId",
                        column: x => x.IpcId,
                        principalTable: "DicIPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IPC_ProtectionDoc_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDoc_Bulletin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BulletinId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDoc_Bulletin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDoc_Bulletin_Bulletin_BulletinId",
                        column: x => x.BulletinId,
                        principalTable: "Bulletin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDoc_Bulletin_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDoc_ProtectionDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDoc_ProtectionDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDoc_ProtectionDoc_ProtectionDocs_ChildId",
                        column: x => x.ChildId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDoc_ProtectionDoc_ProtectionDocs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocConventionInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateEurasianApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateInternationalApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EarlyRegTypeId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    HeadIps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternationalAppToNationalPhaseTransferDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    PublishDateEurasianApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PublishDateInternationalApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PublishRegNumberEurasianApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishRegNumberInternationalApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegNumberEurasianApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegNumberInternationalApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermNationalPhaseFirsChapter = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TermNationalPhaseSecondChapter = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocConventionInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocConventionInfos_DicCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocConventionInfos_DicEarlyRegTypes_EarlyRegTypeId",
                        column: x => x.EarlyRegTypeId,
                        principalTable: "DicEarlyRegTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocConventionInfos_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerRoleId = table.Column<int>(type: "int", nullable: true),
                    DateBegin = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneFax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocCustomers_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocCustomers_DicCustomerRoles_CustomerRoleId",
                        column: x => x.CustomerRoleId,
                        principalTable: "DicCustomerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocCustomers_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocDocuments_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocEarlyRegs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateF1 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateF2 = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarlyRegTypeId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    NameSD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCTType = table.Column<int>(type: "int", nullable: true),
                    PriorityDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    RegCountryId = table.Column<int>(type: "int", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocEarlyRegs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocEarlyRegs_DicEarlyRegTypes_EarlyRegTypeId",
                        column: x => x.EarlyRegTypeId,
                        principalTable: "DicEarlyRegTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocEarlyRegs_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocEarlyRegs_DicCountries_RegCountryId",
                        column: x => x.RegCountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcceptAgreement = table.Column<bool>(type: "bit", nullable: true),
                    Breed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreedCountryId = table.Column<int>(type: "int", nullable: true),
                    BreedingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FlagHeirship = table.Column<bool>(type: "bit", nullable: false),
                    FlagNine = table.Column<bool>(type: "bit", nullable: false),
                    FlagTat = table.Column<bool>(type: "bit", nullable: false),
                    FlagTpt = table.Column<bool>(type: "bit", nullable: false),
                    FlagTth = table.Column<bool>(type: "bit", nullable: false),
                    FlagTtw = table.Column<bool>(type: "bit", nullable: false),
                    Genus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsColorPerformance = table.Column<bool>(type: "bit", nullable: true),
                    IsConventionPriority = table.Column<bool>(type: "bit", nullable: true),
                    IsExhibitPriority = table.Column<bool>(type: "bit", nullable: true),
                    IsStandardFont = table.Column<bool>(type: "bit", nullable: true),
                    IsVolumeTZ = table.Column<bool>(type: "bit", nullable: true),
                    IzCollectiveTZ = table.Column<bool>(type: "bit", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductSpecialProp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transliteration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocInfos_DicCountries_BreedCountryId",
                        column: x => x.BreedCountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocInfos_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocRedefines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    RedefinitionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RedefinitionTypeId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocRedefines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocRedefines_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocRedefines_DicRedefinitionTypes_RedefinitionTypeId",
                        column: x => x.RedefinitionTypeId,
                        principalTable: "DicRedefinitionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProtectionDocWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ControlDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CurrentStageId = table.Column<int>(type: "int", nullable: true),
                    CurrentUserId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateReceived = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FromStageId = table.Column<int>(type: "int", nullable: true),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    SecondaryCurrentUserId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtectionDocWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtectionDocWorkflows_DicRouteStages_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocWorkflows_AspNetUsers_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocWorkflows_DicRouteStages_FromStageId",
                        column: x => x.FromStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocWorkflows_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocWorkflows_ProtectionDocs_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocWorkflows_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtectionDocWorkflows_AspNetUsers_SecondaryCurrentUserId",
                        column: x => x.SecondaryCurrentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract_ProtectionDoc_ICGSProtectionDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractProtectionDocRelationId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ICGSProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract_ProtectionDoc_ICGSProtectionDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_ProtectionDoc_ICGSProtectionDoc_Contract_ProtectionDoc_ContractProtectionDocRelationId",
                        column: x => x.ContractProtectionDocRelationId,
                        principalTable: "Contract_ProtectionDoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contract_ProtectionDoc_ICGSProtectionDoc_ICGS_ProtectionDoc_ICGSProtectionDocId",
                        column: x => x.ICGSProtectionDocId,
                        principalTable: "ICGS_ProtectionDoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddresseeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddresseeId = table.Column<int>(type: "int", nullable: true),
                    ApplicantTypeId = table.Column<int>(type: "int", nullable: true),
                    Barcode = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryTypeId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoefficientComplexity = table.Column<double>(type: "float", nullable: true),
                    ConventionTypeId = table.Column<int>(type: "int", nullable: true),
                    CopyCount = table.Column<int>(type: "int", nullable: true),
                    CountIndependentItems = table.Column<int>(type: "int", nullable: true),
                    CurrentWorkflowId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateRecognizedKnown = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclaimerKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclaimerRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistributionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DivisionId = table.Column<int>(type: "int", nullable: true),
                    ExpertSearchKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FlDivisionId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IncomingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncomingNumberFilial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InfoConfirmKnownTrademark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InfoDecisionToRecognizedKnown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDocSendToEmail = table.Column<bool>(type: "bit", nullable: true),
                    IsImageFromName = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    MainAttachmentId = table.Column<int>(type: "int", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberBulletin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutgoingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OutgoingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    PreviewImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProductPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: false),
                    PublicDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
                    Referat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RequestInfoId = table.Column<int>(type: "int", nullable: true),
                    RequestNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestTypeId = table.Column<int>(type: "int", nullable: true),
                    RomarinColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScanFileId = table.Column<int>(type: "int", nullable: true),
                    SelectionAchieveTypeId = table.Column<int>(type: "int", nullable: true),
                    SelectionFamily = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StatusSending = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateDataFileId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TransferDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Transliteration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeTrademarkId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_DicCustomers_AddresseeId",
                        column: x => x.AddresseeId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicApplicantTypes_ApplicantTypeId",
                        column: x => x.ApplicantTypeId,
                        principalTable: "DicApplicantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicBeneficiaryTypes_BeneficiaryTypeId",
                        column: x => x.BeneficiaryTypeId,
                        principalTable: "DicBeneficiaryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicConventionTypes_ConventionTypeId",
                        column: x => x.ConventionTypeId,
                        principalTable: "DicConventionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DicDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicDivisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "DicDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicDivisions_FlDivisionId",
                        column: x => x.FlDivisionId,
                        principalTable: "DicDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Attachments_MainAttachmentId",
                        column: x => x.MainAttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicProtectionDocTypes_ProtectionDocTypeId",
                        column: x => x.ProtectionDocTypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicReceiveTypes_ReceiveTypeId",
                        column: x => x.ReceiveTypeId,
                        principalTable: "DicReceiveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicProtectionDocSubTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "DicProtectionDocSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicSelectionAchieveTypes_SelectionAchieveTypeId",
                        column: x => x.SelectionAchieveTypeId,
                        principalTable: "DicSelectionAchieveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicRequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DicRequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_DicTypeTrademarks_TypeTrademarkId",
                        column: x => x.TypeTrademarkId,
                        principalTable: "DicTypeTrademarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalDocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    GazetteReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntRegisterEffectiveDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IntRegisterRegnDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NotificationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OfficeOfOriginCountryId = table.Column<int>(type: "int", nullable: true),
                    PublicationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalDocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalDocs_DicCountries_OfficeOfOriginCountryId",
                        column: x => x.OfficeOfOriginCountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdditionalDocs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract_Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_Request_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Request_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DicColorTZ_Request",
                columns: table => new
                {
                    ColorTzId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicColorTZ_Request", x => new { x.ColorTzId, x.RequestId });
                    table.ForeignKey(
                        name: "FK_DicColorTZ_Request_DicColorTZs_ColorTzId",
                        column: x => x.ColorTzId,
                        principalTable: "DicColorTZs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicColorTZ_Request_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DicIcfem_Request",
                columns: table => new
                {
                    DicIcfemId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicIcfem_Request", x => new { x.DicIcfemId, x.RequestId });
                    table.ForeignKey(
                        name: "FK_DicIcfem_Request_DicICFEMs_DicIcfemId",
                        column: x => x.DicIcfemId,
                        principalTable: "DicICFEMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DicIcfem_Request_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpertSearchSimilarities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ImageSimilarity = table.Column<int>(type: "int", nullable: false),
                    OwnerType = table.Column<int>(type: "int", nullable: false),
                    PhonSimilarity = table.Column<int>(type: "int", nullable: false),
                    ProtectionDocCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocFormula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    SemSimilarity = table.Column<int>(type: "int", nullable: false),
                    SimilarProtectionDocId = table.Column<int>(type: "int", nullable: true),
                    SimilarRequestId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertSearchSimilarities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpertSearchSimilarities_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpertSearchSimilarities_ProtectionDocs_SimilarProtectionDocId",
                        column: x => x.SimilarProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpertSearchSimilarities_Requests_SimilarRequestId",
                        column: x => x.SimilarRequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpertSearchView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AddressForCorrespondence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<int>(type: "int", nullable: false),
                    Confidant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Declarant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclaimerKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisclaimerRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarlyTerminationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExtensionDateTz = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Formula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GosDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    GosNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icfems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icgs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ipcs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberBulletin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerType = table.Column<int>(type: "int", nullable: false),
                    PatentAttorney = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatentOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviewImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PriorityData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityRegCountryNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityRegNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: true),
                    ProtectionDocTypeId = table.Column<int>(type: "int", nullable: false),
                    PublicDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
                    ReceiveTypeNameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    RequestNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestTypeId = table.Column<int>(type: "int", nullable: true),
                    RequestTypeNameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchStatus = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StatusNameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Transliteration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertSearchView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpertSearchView_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpertSearchView_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ICGS_Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimedDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IcgsId = table.Column<int>(type: "int", nullable: false),
                    IsNegative = table.Column<bool>(type: "bit", nullable: true),
                    IsNegativePartial = table.Column<bool>(type: "bit", nullable: true),
                    IsRefused = table.Column<bool>(type: "bit", nullable: true),
                    NegativeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICGS_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ICGS_Request_DicICGSs_IcgsId",
                        column: x => x.IcgsId,
                        principalTable: "DicICGSs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ICGS_Request_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ICIS_Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IcisId = table.Column<int>(type: "int", nullable: false),
                    ImportedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICIS_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ICIS_Request_DicICISs_IcisId",
                        column: x => x.IcisId,
                        principalTable: "DicICISs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ICIS_Request_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntellectualProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abstract = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    ApplicantTypeId = table.Column<int>(type: "int", nullable: false),
                    Attorney = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BulletinDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confidant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConventionTypeId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeclaredName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EarlyTerminationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExtensionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FirstPubInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GosNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    IssuePatentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberApxWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberBulletin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberCopyrightCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaperworkStateRegister = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patentee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocSubTypeId = table.Column<int>(type: "int", nullable: true),
                    RecoveryPetitionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RefusalToPublish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Transliteration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ValidDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntellectualProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntellectualProperties_DicAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "DicAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntellectualProperties_DicApplicantTypes_ApplicantTypeId",
                        column: x => x.ApplicantTypeId,
                        principalTable: "DicApplicantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntellectualProperties_DicConventionTypes_ConventionTypeId",
                        column: x => x.ConventionTypeId,
                        principalTable: "DicConventionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntellectualProperties_DicProtectionDocSubTypes_ProtectionDocSubTypeId",
                        column: x => x.ProtectionDocSubTypeId,
                        principalTable: "DicProtectionDocSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntellectualProperties_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntellectualProperties_DicIntellectualPropertyStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DicIntellectualPropertyStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntellectualProperties_DicProtectionDocTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DicProtectionDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IPC_Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IpcId = table.Column<int>(type: "int", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPC_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IPC_Request_DicIPCs_IpcId",
                        column: x => x.IpcId,
                        principalTable: "DicIPCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IPC_Request_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTaskQueues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ConditionStageId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DicCustomerId = table.Column<int>(type: "int", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsExecuted = table.Column<bool>(type: "bit", nullable: true),
                    IsSms = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    ResolveDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTaskQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTaskQueues_DicRouteStages_ConditionStageId",
                        column: x => x.ConditionStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTaskQueues_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTaskQueues_DicCustomers_DicCustomerId",
                        column: x => x.DicCustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTaskQueues_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTaskQueues_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTaskQueues_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Request_Request",
                columns: table => new
                {
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsAnswer = table.Column<bool>(type: "bit", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request_Request", x => new { x.ChildId, x.ParentId });
                    table.ForeignKey(
                        name: "FK_Request_Request_Requests_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_Request_Requests_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestConventionInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateEurasianApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateInternationalApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EarlyRegTypeId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    HeadIps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternationalAppToNationalPhaseTransferDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PublishDateEurasianApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PublishDateInternationalApp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PublishRegNumberEurasianApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishRegNumberInternationalApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegNumberEurasianApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegNumberInternationalApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    TermNationalPhaseFirsChapter = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TermNationalPhaseSecondChapter = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestConventionInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestConventionInfos_DicCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestConventionInfos_DicEarlyRegTypes_EarlyRegTypeId",
                        column: x => x.EarlyRegTypeId,
                        principalTable: "DicEarlyRegTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestConventionInfos_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerRoleId = table.Column<int>(type: "int", nullable: true),
                    DateBegin = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneFax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestCustomers_DicCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "DicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestCustomers_DicCustomerRoles_CustomerRoleId",
                        column: x => x.CustomerRoleId,
                        principalTable: "DicCustomerRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestCustomers_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestEarlyRegs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EarlyRegTypeId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ITMRawPriorityData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameSD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegCountryId = table.Column<int>(type: "int", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StageSD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestEarlyRegs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestEarlyRegs_DicEarlyRegTypes_EarlyRegTypeId",
                        column: x => x.EarlyRegTypeId,
                        principalTable: "DicEarlyRegTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestEarlyRegs_DicCountries_RegCountryId",
                        column: x => x.RegCountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestEarlyRegs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AcceptAgreement = table.Column<bool>(type: "bit", nullable: true),
                    Breed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreedCountryId = table.Column<int>(type: "int", nullable: true),
                    BreedingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FlagHeirship = table.Column<bool>(type: "bit", nullable: false),
                    FlagNine = table.Column<bool>(type: "bit", nullable: false),
                    FlagTat = table.Column<bool>(type: "bit", nullable: false),
                    FlagTpt = table.Column<bool>(type: "bit", nullable: false),
                    FlagTth = table.Column<bool>(type: "bit", nullable: false),
                    FlagTtw = table.Column<bool>(type: "bit", nullable: false),
                    Genus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsColorPerformance = table.Column<bool>(type: "bit", nullable: true),
                    IsConventionPriority = table.Column<bool>(type: "bit", nullable: true),
                    IsExhibitPriority = table.Column<bool>(type: "bit", nullable: true),
                    IsStandardFont = table.Column<bool>(type: "bit", nullable: true),
                    IsVolumeTZ = table.Column<bool>(type: "bit", nullable: true),
                    IzCollectiveTZ = table.Column<bool>(type: "bit", nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductSpecialProp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transliteration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestInfos_DicCountries_BreedCountryId",
                        column: x => x.BreedCountryId,
                        principalTable: "DicCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestInfos_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestProtectionDocSimilarities",
                columns: table => new
                {
                    ProtectionDocId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestProtectionDocSimilarities", x => new { x.ProtectionDocId, x.RequestId });
                    table.ForeignKey(
                        name: "FK_RequestProtectionDocSimilarities_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestProtectionDocSimilarities_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestsDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestsDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestsDocuments_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestsNotificationStatuses",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    NotificationStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestsNotificationStatuses", x => new { x.RequestId, x.NotificationStatusId });
                    table.ForeignKey(
                        name: "FK_RequestsNotificationStatuses_DicNotificationStatuses_NotificationStatusId",
                        column: x => x.NotificationStatusId,
                        principalTable: "DicNotificationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestsNotificationStatuses_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestWorkflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ControlDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CurrentStageId = table.Column<int>(type: "int", nullable: true),
                    CurrentUserId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateReceived = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    FromStageId = table.Column<int>(type: "int", nullable: true),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestWorkflows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestWorkflows_DicRouteStages_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestWorkflows_AspNetUsers_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestWorkflows_DicRouteStages_FromStageId",
                        column: x => x.FromStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestWorkflows_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestWorkflows_Requests_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestWorkflows_DicRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DicRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GosDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    GosNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReqNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Xin = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statements_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTaskQueues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConditionStageId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IsExecuted = table.Column<bool>(type: "bit", nullable: true),
                    ProtectionDocId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    ResolveDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ResultStageId = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTaskQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskQueues_DicRouteStages_ConditionStageId",
                        column: x => x.ConditionStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskQueues_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskQueues_ProtectionDocs_ProtectionDocId",
                        column: x => x.ProtectionDocId,
                        principalTable: "ProtectionDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskQueues_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskQueues_DicRouteStages_ResultStageId",
                        column: x => x.ResultStageId,
                        principalTable: "DicRouteStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract_Request_ICGSRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractRequestRelationId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    ICGSRequestId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract_Request_ICGSRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_Request_ICGSRequest_Contract_Request_ContractRequestRelationId",
                        column: x => x.ContractRequestRelationId,
                        principalTable: "Contract_Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Request_ICGSRequest_ICGS_Request_ICGSRequestId",
                        column: x => x.ICGSRequestId,
                        principalTable: "ICGS_Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalDocs_OfficeOfOriginCountryId",
                table: "AdditionalDocs",
                column: "OfficeOfOriginCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalDocs_RequestId",
                table: "AdditionalDocs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserICGSs_IcgsId",
                table: "AspNetUserICGSs",
                column: "IcgsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserIpcs_IpcId",
                table: "AspNetUserIpcs",
                column: "IpcId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PositionId",
                table: "AspNetUsers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_AuthorId",
                table: "Attachments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_DocumentId",
                table: "Attachments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityCorrespondences_DocumentTypeId",
                table: "AvailabilityCorrespondences",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityCorrespondences_ProtectionDocTypeId",
                table: "AvailabilityCorrespondences",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityCorrespondences_RouteStageId",
                table: "AvailabilityCorrespondences",
                column: "RouteStageId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_EventTypeId",
                table: "CalendarEvents",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_DicCustomerId",
                table: "ContactInfos",
                column: "DicCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfos_TypeId",
                table: "ContactInfos",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ProtectionDoc_ContractId",
                table: "Contract_ProtectionDoc",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ProtectionDoc_ProtectionDocId",
                table: "Contract_ProtectionDoc",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ProtectionDoc_ICGSProtectionDoc_ContractProtectionDocRelationId",
                table: "Contract_ProtectionDoc_ICGSProtectionDoc",
                column: "ContractProtectionDocRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ProtectionDoc_ICGSProtectionDoc_ICGSProtectionDocId",
                table: "Contract_ProtectionDoc_ICGSProtectionDoc",
                column: "ICGSProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Request_ContractId",
                table: "Contract_Request",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Request_RequestId",
                table: "Contract_Request",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Request_ICGSRequest_ContractRequestRelationId",
                table: "Contract_Request_ICGSRequest",
                column: "ContractRequestRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Request_ICGSRequest_ICGSRequestId",
                table: "Contract_Request_ICGSRequest",
                column: "ICGSRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractCustomers_ContractId",
                table: "ContractCustomers",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractCustomers_CustomerId",
                table: "ContractCustomers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractCustomers_CustomerRoleId",
                table: "ContractCustomers",
                column: "CustomerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_AddresseeId",
                table: "Contracts",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ApplicantTypeId",
                table: "Contracts",
                column: "ApplicantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CategoryId",
                table: "Contracts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractTypeId",
                table: "Contracts",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CurrentWorkflowId",
                table: "Contracts",
                column: "CurrentWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DepartmentId",
                table: "Contracts",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DivisionId",
                table: "Contracts",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_FullExpertiseExecutorId",
                table: "Contracts",
                column: "FullExpertiseExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_MainAttachmentId",
                table: "Contracts",
                column: "MainAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProtectionDocTypeId",
                table: "Contracts",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ReceiveTypeId",
                table: "Contracts",
                column: "ReceiveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_StatusId",
                table: "Contracts",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsDocuments_ContractId",
                table: "ContractsDocuments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsDocuments_DocumentId",
                table: "ContractsDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsNotificationStatuses_NotificationStatusId",
                table: "ContractsNotificationStatuses",
                column: "NotificationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractWorkflows_CurrentStageId",
                table: "ContractWorkflows",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractWorkflows_CurrentUserId",
                table: "ContractWorkflows",
                column: "CurrentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractWorkflows_FromStageId",
                table: "ContractWorkflows",
                column: "FromStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractWorkflows_FromUserId",
                table: "ContractWorkflows",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractWorkflows_OwnerId",
                table: "ContractWorkflows",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractWorkflows_RouteId",
                table: "ContractWorkflows",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAttorneyInfos_CustomerId",
                table: "CustomerAttorneyInfos",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DicAddresses_ContinentId",
                table: "DicAddresses",
                column: "ContinentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicAddresses_CountryId",
                table: "DicAddresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DicAddresses_LocationId",
                table: "DicAddresses",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DicColorTZ_ProtectionDoc_ProtectionDocId",
                table: "DicColorTZ_ProtectionDoc",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_DicColorTZ_Request_RequestId",
                table: "DicColorTZ_Request",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DicConsiderationTypes_ProtectionDocTypeId",
                table: "DicConsiderationTypes",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicContinents_ParentId",
                table: "DicContinents",
                column: "ParentId");

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

            migrationBuilder.CreateIndex(
                name: "IX_DicCountries_ContinentId",
                table: "DicCountries",
                column: "ContinentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicCountries_ParentId",
                table: "DicCountries",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicCustomers_BeneficiaryTypeId",
                table: "DicCustomers",
                column: "BeneficiaryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicCustomers_CountryId",
                table: "DicCustomers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DicCustomers_TypeId",
                table: "DicCustomers",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDepartments_DepartmentTypeId",
                table: "DicDepartments",
                column: "DepartmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDepartments_DivisionId",
                table: "DicDepartments",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDetailICGSs_IcgsId",
                table: "DicDetailICGSs",
                column: "IcgsId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDocumentClassifications_ParentId",
                table: "DicDocumentClassifications",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDocumentTypes_ClassificationId",
                table: "DicDocumentTypes",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDocumentTypes_RouteId",
                table: "DicDocumentTypes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDocumentTypes_TemplateFileId",
                table: "DicDocumentTypes",
                column: "TemplateFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DicDocumentTypesDicProtectionDocTypes_DicProtectionDocTypeId",
                table: "DicDocumentTypesDicProtectionDocTypes",
                column: "DicProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicEarlyRegTypes_ProtectionDocTypeId",
                table: "DicEarlyRegTypes",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicEntityAccessTypes_DocumentAccessPermissionsId",
                table: "DicEntityAccessTypes",
                column: "DocumentAccessPermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_DicIcfem_ProtectionDoc_ProtectionDocId",
                table: "DicIcfem_ProtectionDoc",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_DicIcfem_Request_RequestId",
                table: "DicIcfem_Request",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DicICFEMs_ParentId",
                table: "DicICFEMs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicICISs_ParentId",
                table: "DicICISs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicIPCs_ParentId",
                table: "DicIPCs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicLocations_CountryId",
                table: "DicLocations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DicLocations_ParentId",
                table: "DicLocations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicLocations_TypeId",
                table: "DicLocations",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicPositions_DepartmentId",
                table: "DicPositions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicProtectionDocSubTypes_TypeId",
                table: "DicProtectionDocSubTypes",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicProtectionDocTypes_DepartmentId",
                table: "DicProtectionDocTypes",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DicProtectionDocTypes_RouteId",
                table: "DicProtectionDocTypes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRequisitionFeedTypes_ProtectionDocTypeId",
                table: "DicRequisitionFeedTypes",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_ContractStatusId",
                table: "DicRouteStages",
                column: "ContractStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_FinishConServiceStatusId",
                table: "DicRouteStages",
                column: "FinishConServiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_OnlineRequisitionStatusId",
                table: "DicRouteStages",
                column: "OnlineRequisitionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_ProtectionDocStatusId",
                table: "DicRouteStages",
                column: "ProtectionDocStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_RequestStatusId",
                table: "DicRouteStages",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_RouteId",
                table: "DicRouteStages",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_DicRouteStages_StartConServiceStatusId",
                table: "DicRouteStages",
                column: "StartConServiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DicTariffs_NiisTariffId",
                table: "DicTariffs",
                column: "NiisTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_DicTariffs_ProtectionDocTypeId",
                table: "DicTariffs",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DicTariffs_ReceiveTypeId",
                table: "DicTariffs",
                column: "ReceiveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_Customer_AddressId",
                table: "Document_Customer",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_Customer_CustomerId",
                table: "Document_Customer",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_Customer_CustomerRoleId",
                table: "Document_Customer",
                column: "CustomerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_Customer_DocumentId",
                table: "Document_Customer",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessRoles_ApplicationUserId",
                table: "DocumentAccessRoles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessRoles_ClassificationId",
                table: "DocumentAccessRoles",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAccessRoles_TypeId",
                table: "DocumentAccessRoles",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentContents_DocumentId",
                table: "DocumentContents",
                column: "DocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDocumentRelations_ChildId",
                table: "DocumentDocumentRelations",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDocumentRelations_ParentId",
                table: "DocumentDocumentRelations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentEarlyRegs_CountryId",
                table: "DocumentEarlyRegs",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentEarlyRegs_DocumentId",
                table: "DocumentEarlyRegs",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentEarlyRegs_EarlyRegTypeId",
                table: "DocumentEarlyRegs",
                column: "EarlyRegTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentExecutors_DocumentId",
                table: "DocumentExecutors",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentExecutors_UserId",
                table: "DocumentExecutors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProperties_SendTypeId",
                table: "DocumentProperties",
                column: "SendTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AddresseeId",
                table: "Documents",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_BulletinId",
                table: "Documents",
                column: "BulletinId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CurrentWorkflowId",
                table: "Documents",
                column: "CurrentWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DepartmentId",
                table: "Documents",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DivisionId",
                table: "Documents",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MainAttachmentId",
                table: "Documents",
                column: "MainAttachmentId",
                unique: true,
                filter: "[MainAttachmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ProtectionDocTypeId",
                table: "Documents",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ReceiveTypeId",
                table: "Documents",
                column: "ReceiveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TypeId",
                table: "Documents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsNotificationStatuses_NotificationStatusId",
                table: "DocumentsNotificationStatuses",
                column: "NotificationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUserInputs_DocumentId",
                table: "DocumentUserInputs",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUserSignatures_UserId",
                table: "DocumentUserSignatures",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUserSignatures_WorkflowId",
                table: "DocumentUserSignatures",
                column: "WorkflowId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflows_CurrentStageId",
                table: "DocumentWorkflows",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflows_CurrentUserId",
                table: "DocumentWorkflows",
                column: "CurrentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflows_FromStageId",
                table: "DocumentWorkflows",
                column: "FromStageId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflows_FromUserId",
                table: "DocumentWorkflows",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflows_OwnerId",
                table: "DocumentWorkflows",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentWorkflows_RouteId",
                table: "DocumentWorkflows",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertSearchSimilarities_RequestId",
                table: "ExpertSearchSimilarities",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertSearchSimilarities_SimilarProtectionDocId",
                table: "ExpertSearchSimilarities",
                column: "SimilarProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertSearchSimilarities_SimilarRequestId",
                table: "ExpertSearchSimilarities",
                column: "SimilarRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertSearchView_ProtectionDocId",
                table: "ExpertSearchView",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertSearchView_RequestId",
                table: "ExpertSearchView",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedInvoiceNumbers_DepartmentId",
                table: "GeneratedInvoiceNumbers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedInvoiceNumbers_DocumentId",
                table: "GeneratedInvoiceNumbers",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedInvoiceNumbers_UserId",
                table: "GeneratedInvoiceNumbers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedQueryExpDeps_DepartmentId",
                table: "GeneratedQueryExpDeps",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedQueryExpDeps_DocumentId",
                table: "GeneratedQueryExpDeps",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_GridPrintSettings_UserId",
                table: "GridPrintSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ICGS_ProtectionDoc_IcgsId",
                table: "ICGS_ProtectionDoc",
                column: "IcgsId");

            migrationBuilder.CreateIndex(
                name: "IX_ICGS_ProtectionDoc_ProtectionDocId",
                table: "ICGS_ProtectionDoc",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ICGS_Request_IcgsId",
                table: "ICGS_Request",
                column: "IcgsId");

            migrationBuilder.CreateIndex(
                name: "IX_ICGS_Request_RequestId",
                table: "ICGS_Request",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ICIS_ProtectionDoc_IcisId",
                table: "ICIS_ProtectionDoc",
                column: "IcisId");

            migrationBuilder.CreateIndex(
                name: "IX_ICIS_ProtectionDoc_ProtectionDocId",
                table: "ICIS_ProtectionDoc",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ICIS_Request_IcisId",
                table: "ICIS_Request",
                column: "IcisId");

            migrationBuilder.CreateIndex(
                name: "IX_ICIS_Request_RequestId",
                table: "ICIS_Request",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationConPackages_StateId",
                table: "IntegrationConPackages",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationConPackages_TypeId",
                table: "IntegrationConPackages",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationRequisitions_OnlineRequisitionStatusId",
                table: "IntegrationRequisitions",
                column: "OnlineRequisitionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationRequisitions_ProtectionDocTypeId",
                table: "IntegrationRequisitions",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationStatuses_OnlineRequisitionStatusId",
                table: "IntegrationStatuses",
                column: "OnlineRequisitionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IntellectualProperties_AddressId",
                table: "IntellectualProperties",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_IntellectualProperties_ApplicantTypeId",
                table: "IntellectualProperties",
                column: "ApplicantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntellectualProperties_ConventionTypeId",
                table: "IntellectualProperties",
                column: "ConventionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntellectualProperties_ProtectionDocSubTypeId",
                table: "IntellectualProperties",
                column: "ProtectionDocSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IntellectualProperties_RequestId",
                table: "IntellectualProperties",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_IntellectualProperties_StatusId",
                table: "IntellectualProperties",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_IntellectualProperties_TypeId",
                table: "IntellectualProperties",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IPC_ProtectionDoc_IpcId",
                table: "IPC_ProtectionDoc",
                column: "IpcId");

            migrationBuilder.CreateIndex(
                name: "IX_IPC_ProtectionDoc_ProtectionDocId",
                table: "IPC_ProtectionDoc",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_IPC_Request_IpcId",
                table: "IPC_Request",
                column: "IpcId");

            migrationBuilder.CreateIndex(
                name: "IX_IPC_Request_RequestId",
                table: "IPC_Request",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTaskQueues_ConditionStageId",
                table: "NotificationTaskQueues",
                column: "ConditionStageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTaskQueues_ContractId",
                table: "NotificationTaskQueues",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTaskQueues_DicCustomerId",
                table: "NotificationTaskQueues",
                column: "DicCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTaskQueues_DocumentId",
                table: "NotificationTaskQueues",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTaskQueues_ProtectionDocId",
                table: "NotificationTaskQueues",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTaskQueues_RequestId",
                table: "NotificationTaskQueues",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_ApplicantTypeId",
                table: "PaymentInvoices",
                column: "ApplicantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_ContractId",
                table: "PaymentInvoices",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_CreateUserId",
                table: "PaymentInvoices",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_ProtectionDocId",
                table: "PaymentInvoices",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_RequestId",
                table: "PaymentInvoices",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_StatusId",
                table: "PaymentInvoices",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_TariffId",
                table: "PaymentInvoices",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_WhoBoundUserId",
                table: "PaymentInvoices",
                column: "WhoBoundUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInvoices_WriteOffUserId",
                table: "PaymentInvoices",
                column: "WriteOffUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRegistryDatas_DocumentId",
                table: "PaymentRegistryDatas",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRegistryDatas_PaymentInvoiceId",
                table: "PaymentRegistryDatas",
                column: "PaymentInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CustomerId",
                table: "Payments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_CreateUserId",
                table: "PaymentUses",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_PaymentId",
                table: "PaymentUses",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_PaymentInvoiceId",
                table: "PaymentUses",
                column: "PaymentInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDoc_Bulletin_BulletinId",
                table: "ProtectionDoc_Bulletin",
                column: "BulletinId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDoc_Bulletin_ProtectionDocId",
                table: "ProtectionDoc_Bulletin",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDoc_ProtectionDoc_ChildId",
                table: "ProtectionDoc_ProtectionDoc",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDoc_ProtectionDoc_ParentId",
                table: "ProtectionDoc_ProtectionDoc",
                column: "ParentId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocConventionInfos_CountryId",
                table: "ProtectionDocConventionInfos",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocConventionInfos_EarlyRegTypeId",
                table: "ProtectionDocConventionInfos",
                column: "EarlyRegTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocConventionInfos_ProtectionDocId",
                table: "ProtectionDocConventionInfos",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocCustomers_CustomerId",
                table: "ProtectionDocCustomers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocCustomers_CustomerRoleId",
                table: "ProtectionDocCustomers",
                column: "CustomerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocCustomers_ProtectionDocId",
                table: "ProtectionDocCustomers",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocDocuments_DocumentId",
                table: "ProtectionDocDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocDocuments_ProtectionDocId",
                table: "ProtectionDocDocuments",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocEarlyRegs_EarlyRegTypeId",
                table: "ProtectionDocEarlyRegs",
                column: "EarlyRegTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocEarlyRegs_ProtectionDocId",
                table: "ProtectionDocEarlyRegs",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocEarlyRegs_RegCountryId",
                table: "ProtectionDocEarlyRegs",
                column: "RegCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocInfos_BreedCountryId",
                table: "ProtectionDocInfos",
                column: "BreedCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocInfos_ProtectionDocId",
                table: "ProtectionDocInfos",
                column: "ProtectionDocId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocRedefines_ProtectionDocId",
                table: "ProtectionDocRedefines",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocRedefines_RedefinitionTypeId",
                table: "ProtectionDocRedefines",
                column: "RedefinitionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_AddresseeId",
                table: "ProtectionDocs",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_ApplicantTypeId",
                table: "ProtectionDocs",
                column: "ApplicantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_BeneficiaryTypeId",
                table: "ProtectionDocs",
                column: "BeneficiaryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_BulletinUserId",
                table: "ProtectionDocs",
                column: "BulletinUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_ConsiderationTypeId",
                table: "ProtectionDocs",
                column: "ConsiderationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_ConventionTypeId",
                table: "ProtectionDocs",
                column: "ConventionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_CurrentWorkflowId",
                table: "ProtectionDocs",
                column: "CurrentWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_IntellectualPropertyId",
                table: "ProtectionDocs",
                column: "IntellectualPropertyId",
                unique: true,
                filter: "[IntellectualPropertyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_RequestId",
                table: "ProtectionDocs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_SelectionAchieveTypeId",
                table: "ProtectionDocs",
                column: "SelectionAchieveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_SendTypeId",
                table: "ProtectionDocs",
                column: "SendTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_StatusId",
                table: "ProtectionDocs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_SubTypeId",
                table: "ProtectionDocs",
                column: "SubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_SupportUserId",
                table: "ProtectionDocs",
                column: "SupportUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_TypeId",
                table: "ProtectionDocs",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocs_TypeTrademarkId",
                table: "ProtectionDocs",
                column: "TypeTrademarkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_CurrentStageId",
                table: "ProtectionDocWorkflows",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_CurrentUserId",
                table: "ProtectionDocWorkflows",
                column: "CurrentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_FromStageId",
                table: "ProtectionDocWorkflows",
                column: "FromStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_FromUserId",
                table: "ProtectionDocWorkflows",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_OwnerId",
                table: "ProtectionDocWorkflows",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_RouteId",
                table: "ProtectionDocWorkflows",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProtectionDocWorkflows_SecondaryCurrentUserId",
                table: "ProtectionDocWorkflows",
                column: "SecondaryCurrentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_Request_ParentId",
                table: "Request_Request",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestConventionInfos_CountryId",
                table: "RequestConventionInfos",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestConventionInfos_EarlyRegTypeId",
                table: "RequestConventionInfos",
                column: "EarlyRegTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestConventionInfos_RequestId",
                table: "RequestConventionInfos",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCustomers_CustomerId",
                table: "RequestCustomers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCustomers_CustomerRoleId",
                table: "RequestCustomers",
                column: "CustomerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCustomers_RequestId",
                table: "RequestCustomers",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestEarlyRegs_EarlyRegTypeId",
                table: "RequestEarlyRegs",
                column: "EarlyRegTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestEarlyRegs_RegCountryId",
                table: "RequestEarlyRegs",
                column: "RegCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestEarlyRegs_RequestId",
                table: "RequestEarlyRegs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestInfos_BreedCountryId",
                table: "RequestInfos",
                column: "BreedCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestInfos_RequestId",
                table: "RequestInfos",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestProtectionDocSimilarities_RequestId",
                table: "RequestProtectionDocSimilarities",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AddresseeId",
                table: "Requests",
                column: "AddresseeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ApplicantTypeId",
                table: "Requests",
                column: "ApplicantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_BeneficiaryTypeId",
                table: "Requests",
                column: "BeneficiaryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ConventionTypeId",
                table: "Requests",
                column: "ConventionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrentWorkflowId",
                table: "Requests",
                column: "CurrentWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DepartmentId",
                table: "Requests",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DivisionId",
                table: "Requests",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_FlDivisionId",
                table: "Requests",
                column: "FlDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_MainAttachmentId",
                table: "Requests",
                column: "MainAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ProtectionDocTypeId",
                table: "Requests",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ReceiveTypeId",
                table: "Requests",
                column: "ReceiveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestTypeId",
                table: "Requests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SelectionAchieveTypeId",
                table: "Requests",
                column: "SelectionAchieveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StatusId",
                table: "Requests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TypeTrademarkId",
                table: "Requests",
                column: "TypeTrademarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestsDocuments_DocumentId",
                table: "RequestsDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestsDocuments_RequestId",
                table: "RequestsDocuments",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestsNotificationStatuses_NotificationStatusId",
                table: "RequestsNotificationStatuses",
                column: "NotificationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_CurrentStageId",
                table: "RequestWorkflows",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_CurrentUserId",
                table: "RequestWorkflows",
                column: "CurrentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_FromStageId",
                table: "RequestWorkflows",
                column: "FromStageId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_FromUserId",
                table: "RequestWorkflows",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_OwnerId",
                table: "RequestWorkflows",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_RouteId",
                table: "RequestWorkflows",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ProtectionDocType_ProtectionDocTypeId",
                table: "Role_ProtectionDocType",
                column: "ProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_RouteStage_StageId",
                table: "Role_RouteStage",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStageOrders_CurrentStageId",
                table: "RouteStageOrders",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStageOrders_NextStageId",
                table: "RouteStageOrders",
                column: "NextStageId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingGridOptions_UserId",
                table: "SettingGridOptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_RequestId",
                table: "Statements",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RouteStage_StageId",
                table: "User_RouteStage",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskQueues_ConditionStageId",
                table: "WorkflowTaskQueues",
                column: "ConditionStageId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskQueues_ContractId",
                table: "WorkflowTaskQueues",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskQueues_ProtectionDocId",
                table: "WorkflowTaskQueues",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskQueues_RequestId",
                table: "WorkflowTaskQueues",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskQueues_ResultStageId",
                table: "WorkflowTaskQueues",
                column: "ResultStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Documents_DocumentId",
                table: "Attachments",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractWorkflows_CurrentWorkflowId",
                table: "Contracts",
                column: "CurrentWorkflowId",
                principalTable: "ContractWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentExecutors_Documents_DocumentId",
                table: "DocumentExecutors",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentUserSignatures_DocumentWorkflows_WorkflowId",
                table: "DocumentUserSignatures",
                column: "WorkflowId",
                principalTable: "DocumentWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentWorkflows_Documents_OwnerId",
                table: "DocumentWorkflows",
                column: "OwnerId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoices_Requests_RequestId",
                table: "PaymentInvoices",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInvoices_ProtectionDocs_ProtectionDocId",
                table: "PaymentInvoices",
                column: "ProtectionDocId",
                principalTable: "ProtectionDocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtectionDocs_Requests_RequestId",
                table: "ProtectionDocs",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtectionDocs_ProtectionDocWorkflows_CurrentWorkflowId",
                table: "ProtectionDocs",
                column: "CurrentWorkflowId",
                principalTable: "ProtectionDocWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtectionDocs_IntellectualProperties_IntellectualPropertyId",
                table: "ProtectionDocs",
                column: "IntellectualPropertyId",
                principalTable: "IntellectualProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestWorkflows_CurrentWorkflowId",
                table: "Requests",
                column: "CurrentWorkflowId",
                principalTable: "RequestWorkflows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DicAddresses_DicCountries_CountryId",
                table: "DicAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_DicCustomers_DicCountries_CountryId",
                table: "DicCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_DicLocations_DicCountries_CountryId",
                table: "DicLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_IntellectualProperties_Requests_RequestId",
                table: "IntellectualProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocs_Requests_RequestId",
                table: "ProtectionDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestWorkflows_Requests_OwnerId",
                table: "RequestWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_AspNetUsers_AuthorId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_AspNetUsers_FullExpertiseExecutorId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractWorkflows_AspNetUsers_CurrentUserId",
                table: "ContractWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractWorkflows_AspNetUsers_FromUserId",
                table: "ContractWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentWorkflows_AspNetUsers_CurrentUserId",
                table: "DocumentWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentWorkflows_AspNetUsers_FromUserId",
                table: "DocumentWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocs_AspNetUsers_BulletinUserId",
                table: "ProtectionDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocs_AspNetUsers_SupportUserId",
                table: "ProtectionDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocWorkflows_AspNetUsers_CurrentUserId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocWorkflows_AspNetUsers_FromUserId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocWorkflows_AspNetUsers_SecondaryCurrentUserId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_DicCustomers_AddresseeId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DicCustomers_AddresseeId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocs_DicCustomers_AddresseeId",
                table: "ProtectionDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_DicDepartments_DepartmentId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DicProtectionDocTypes_DicDepartments_DepartmentId",
                table: "DicProtectionDocTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DicDepartments_DepartmentId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Documents_DocumentId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentWorkflows_Documents_OwnerId",
                table: "DocumentWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_DicProtectionDocTypes_ProtectionDocTypeId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_DicConsiderationTypes_DicProtectionDocTypes_ProtectionDocTypeId",
                table: "DicConsiderationTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_DicProtectionDocSubTypes_DicProtectionDocTypes_TypeId",
                table: "DicProtectionDocSubTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_IntellectualProperties_DicProtectionDocTypes_TypeId",
                table: "IntellectualProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocs_DicProtectionDocTypes_TypeId",
                table: "ProtectionDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractWorkflows_DicRouteStages_CurrentStageId",
                table: "ContractWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractWorkflows_DicRouteStages_FromStageId",
                table: "ContractWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocWorkflows_DicRouteStages_CurrentStageId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocWorkflows_DicRouteStages_FromStageId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractWorkflows_Contracts_OwnerId",
                table: "ContractWorkflows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtectionDocWorkflows_ProtectionDocs_OwnerId",
                table: "ProtectionDocWorkflows");

            migrationBuilder.DropTable(
                name: "AdditionalDocs");

            migrationBuilder.DropTable(
                name: "AspNetClaimConstants");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserICGSs");

            migrationBuilder.DropTable(
                name: "AspNetUserIpcs");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AvailabilityCorrespondences");

            migrationBuilder.DropTable(
                name: "CalendarEvents");

            migrationBuilder.DropTable(
                name: "ContactInfos");

            migrationBuilder.DropTable(
                name: "Contract_ProtectionDoc_ICGSProtectionDoc");

            migrationBuilder.DropTable(
                name: "Contract_Request_ICGSRequest");

            migrationBuilder.DropTable(
                name: "ContractCustomers");

            migrationBuilder.DropTable(
                name: "ContractsDocuments");

            migrationBuilder.DropTable(
                name: "ContractsNotificationStatuses");

            migrationBuilder.DropTable(
                name: "CustomerAttorneyInfos");

            migrationBuilder.DropTable(
                name: "DicColorTZ_ProtectionDoc");

            migrationBuilder.DropTable(
                name: "DicColorTZ_Request");

            migrationBuilder.DropTable(
                name: "DicContractTypes");

            migrationBuilder.DropTable(
                name: "DicDetailICGSs");

            migrationBuilder.DropTable(
                name: "DicDocumentTypesDicProtectionDocTypes");

            migrationBuilder.DropTable(
                name: "DicEntityAccessTypes");

            migrationBuilder.DropTable(
                name: "DicIcfem_ProtectionDoc");

            migrationBuilder.DropTable(
                name: "DicIcfem_Request");

            migrationBuilder.DropTable(
                name: "DicLogTypes");

            migrationBuilder.DropTable(
                name: "DicProtectionDocBulletinTypes");

            migrationBuilder.DropTable(
                name: "DicProtectionDocTMTypes");

            migrationBuilder.DropTable(
                name: "DicRedefinitionDocumentTypes");

            migrationBuilder.DropTable(
                name: "DicRequisitionFeedTypes");

            migrationBuilder.DropTable(
                name: "Document_Customer");

            migrationBuilder.DropTable(
                name: "DocumentContents");

            migrationBuilder.DropTable(
                name: "DocumentDocumentRelations");

            migrationBuilder.DropTable(
                name: "DocumentEarlyRegs");

            migrationBuilder.DropTable(
                name: "DocumentExecutors");

            migrationBuilder.DropTable(
                name: "DocumentProperties");

            migrationBuilder.DropTable(
                name: "DocumentsNotificationStatuses");

            migrationBuilder.DropTable(
                name: "DocumentUserInputs");

            migrationBuilder.DropTable(
                name: "DocumentUserSignatures");

            migrationBuilder.DropTable(
                name: "ExpertSearchSimilarities");

            migrationBuilder.DropTable(
                name: "ExpertSearchView");

            migrationBuilder.DropTable(
                name: "GeneratedInvoiceNumbers");

            migrationBuilder.DropTable(
                name: "GeneratedQueryExpDeps");

            migrationBuilder.DropTable(
                name: "GridPrintSettings");

            migrationBuilder.DropTable(
                name: "ICIS_ProtectionDoc");

            migrationBuilder.DropTable(
                name: "ICIS_Request");

            migrationBuilder.DropTable(
                name: "IntegrationConPackages");

            migrationBuilder.DropTable(
                name: "IntegrationDocuments");

            migrationBuilder.DropTable(
                name: "IntegrationEGovPays");

            migrationBuilder.DropTable(
                name: "IntegrationLogRefLists");

            migrationBuilder.DropTable(
                name: "IntegrationPaymentCalcs");

            migrationBuilder.DropTable(
                name: "IntegrationRequisitions");

            migrationBuilder.DropTable(
                name: "IntegrationRomarinFiles");

            migrationBuilder.DropTable(
                name: "IntegrationRomarinLog");

            migrationBuilder.DropTable(
                name: "IntegrationStatuses");

            migrationBuilder.DropTable(
                name: "IPC_ProtectionDoc");

            migrationBuilder.DropTable(
                name: "IPC_Request");

            migrationBuilder.DropTable(
                name: "NotificationTaskQueues");

            migrationBuilder.DropTable(
                name: "PaymentRegistryDatas");

            migrationBuilder.DropTable(
                name: "PaymentUses");

            migrationBuilder.DropTable(
                name: "ProtectionDoc_Bulletin");

            migrationBuilder.DropTable(
                name: "ProtectionDoc_ProtectionDoc");

            migrationBuilder.DropTable(
                name: "ProtectionDocAttorneys");

            migrationBuilder.DropTable(
                name: "ProtectionDocConventionInfos");

            migrationBuilder.DropTable(
                name: "ProtectionDocCustomers");

            migrationBuilder.DropTable(
                name: "ProtectionDocDocuments");

            migrationBuilder.DropTable(
                name: "ProtectionDocEarlyRegs");

            migrationBuilder.DropTable(
                name: "ProtectionDocInfos");

            migrationBuilder.DropTable(
                name: "ProtectionDocRedefines");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Request_Request");

            migrationBuilder.DropTable(
                name: "RequestConventionInfos");

            migrationBuilder.DropTable(
                name: "RequestCustomers");

            migrationBuilder.DropTable(
                name: "RequestEarlyRegs");

            migrationBuilder.DropTable(
                name: "RequestInfos");

            migrationBuilder.DropTable(
                name: "RequestProtectionDocSimilarities");

            migrationBuilder.DropTable(
                name: "RequestsDocuments");

            migrationBuilder.DropTable(
                name: "RequestsNotificationStatuses");

            migrationBuilder.DropTable(
                name: "Role_ProtectionDocType");

            migrationBuilder.DropTable(
                name: "Role_RouteStage");

            migrationBuilder.DropTable(
                name: "RouteStageOrders");

            migrationBuilder.DropTable(
                name: "SearchView");

            migrationBuilder.DropTable(
                name: "SettingGridOptions");

            migrationBuilder.DropTable(
                name: "Signatures");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "SystemCounter");

            migrationBuilder.DropTable(
                name: "User_RouteStage");

            migrationBuilder.DropTable(
                name: "WorkflowTaskQueues");

            migrationBuilder.DropTable(
                name: "DicEventTypes");

            migrationBuilder.DropTable(
                name: "DicContactInfoTypes");

            migrationBuilder.DropTable(
                name: "Contract_ProtectionDoc");

            migrationBuilder.DropTable(
                name: "ICGS_ProtectionDoc");

            migrationBuilder.DropTable(
                name: "Contract_Request");

            migrationBuilder.DropTable(
                name: "ICGS_Request");

            migrationBuilder.DropTable(
                name: "DicColorTZs");

            migrationBuilder.DropTable(
                name: "DocumentAccessRoles");

            migrationBuilder.DropTable(
                name: "DicICFEMs");

            migrationBuilder.DropTable(
                name: "DicICISs");

            migrationBuilder.DropTable(
                name: "IntegrationConPackageStates");

            migrationBuilder.DropTable(
                name: "IntegrationConPackageTypes");

            migrationBuilder.DropTable(
                name: "DicIPCs");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentInvoices");

            migrationBuilder.DropTable(
                name: "DicRedefinitionTypes");

            migrationBuilder.DropTable(
                name: "DicCustomerRoles");

            migrationBuilder.DropTable(
                name: "DicEarlyRegTypes");

            migrationBuilder.DropTable(
                name: "DicNotificationStatuses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DicICGSs");

            migrationBuilder.DropTable(
                name: "DicPaymentStatuses");

            migrationBuilder.DropTable(
                name: "DicTariffs");

            migrationBuilder.DropTable(
                name: "IntegrationNiisRefTariffs");

            migrationBuilder.DropTable(
                name: "DicCountries");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "RequestWorkflows");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DicPositions");

            migrationBuilder.DropTable(
                name: "DicCustomers");

            migrationBuilder.DropTable(
                name: "DicCustomerTypes");

            migrationBuilder.DropTable(
                name: "DicDepartments");

            migrationBuilder.DropTable(
                name: "DicDepartmentTypes");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Bulletin");

            migrationBuilder.DropTable(
                name: "DocumentWorkflows");

            migrationBuilder.DropTable(
                name: "DicDocumentTypes");

            migrationBuilder.DropTable(
                name: "DicDocumentClassifications");

            migrationBuilder.DropTable(
                name: "DocumentTemplateFiles");

            migrationBuilder.DropTable(
                name: "DicProtectionDocTypes");

            migrationBuilder.DropTable(
                name: "DicRouteStages");

            migrationBuilder.DropTable(
                name: "IntegrationConServiceStatuses");

            migrationBuilder.DropTable(
                name: "DicOnlineRequisitionStatuses");

            migrationBuilder.DropTable(
                name: "DicRequestStatuses");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "DicContractCategories");

            migrationBuilder.DropTable(
                name: "ContractWorkflows");

            migrationBuilder.DropTable(
                name: "DicDivisions");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "DicReceiveTypes");

            migrationBuilder.DropTable(
                name: "DicContractStatuses");

            migrationBuilder.DropTable(
                name: "ProtectionDocs");

            migrationBuilder.DropTable(
                name: "DicBeneficiaryTypes");

            migrationBuilder.DropTable(
                name: "DicConsiderationTypes");

            migrationBuilder.DropTable(
                name: "ProtectionDocWorkflows");

            migrationBuilder.DropTable(
                name: "IntellectualProperties");

            migrationBuilder.DropTable(
                name: "DicSelectionAchieveTypes");

            migrationBuilder.DropTable(
                name: "DicSendTypes");

            migrationBuilder.DropTable(
                name: "DicProtectionDocStatuses");

            migrationBuilder.DropTable(
                name: "DicTypeTrademarks");

            migrationBuilder.DropTable(
                name: "DicRoutes");

            migrationBuilder.DropTable(
                name: "DicAddresses");

            migrationBuilder.DropTable(
                name: "DicApplicantTypes");

            migrationBuilder.DropTable(
                name: "DicConventionTypes");

            migrationBuilder.DropTable(
                name: "DicProtectionDocSubTypes");

            migrationBuilder.DropTable(
                name: "DicIntellectualPropertyStatuses");

            migrationBuilder.DropTable(
                name: "DicContinents");

            migrationBuilder.DropTable(
                name: "DicLocations");

            migrationBuilder.DropTable(
                name: "DicLocationTypes");
        }
    }
}
