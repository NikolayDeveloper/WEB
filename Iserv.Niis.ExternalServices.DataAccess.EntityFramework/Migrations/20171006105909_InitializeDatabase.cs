using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.ExternalServices.DataAccess.EntityFramework.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "IntegrationBulletins",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETDATE()"),
                    Note = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTimeOffset>(nullable: false),
                    Sent = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationBulletins", x => x.Id); });

            migrationBuilder.CreateTable(
                "IntegrationCalcCustomers",
                table => new
                {
                    ActionId = table.Column<int>(nullable: false),
                    SecondPass = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationCalcCustomers", x => new {x.ActionId, x.SecondPass, x.CustomerId});
                });

            migrationBuilder.CreateTable(
                "IntegrationCalcHistories",
                table => new
                {
                    ActionId = table.Column<int>(nullable: false),
                    HistoryId = table.Column<int>(nullable: false),
                    EntityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationCalcHistories", x => new {x.ActionId, x.HistoryId});
                });

            migrationBuilder.CreateTable(
                "IntegrationCalcLinks",
                table => new
                {
                    ActionId = table.Column<int>(nullable: false),
                    PatentId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    LinkId = table.Column<int>(nullable: false),
                    SecondPass = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationCalcLinks",
                        x => new {x.ActionId, x.PatentId, x.CustomerId, x.LinkId, x.SecondPass});
                });

            migrationBuilder.CreateTable(
                "IntegrationCalcPatents",
                table => new
                {
                    ActionId = table.Column<int>(nullable: false),
                    PatentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationCalcPatents", x => new {x.ActionId, x.PatentId});
                });

            migrationBuilder.CreateTable(
                "IntegrationHistories",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerRnn = table.Column<string>(nullable: true),
                    CustomerXin = table.Column<string>(nullable: true),
                    HistoryDate = table.Column<DateTimeOffset>(nullable: false),
                    HistoryEntityType = table.Column<int>(nullable: false),
                    HistoryGRActionType = table.Column<int>(nullable: false),
                    LinkId = table.Column<int>(nullable: true),
                    LinkType = table.Column<int>(nullable: true),
                    PatentDocType = table.Column<int>(nullable: true),
                    PatentEndReason = table.Column<int>(nullable: true),
                    PatentGRRegDate = table.Column<DateTimeOffset>(nullable: true),
                    PatentId = table.Column<int>(nullable: true),
                    PatentName = table.Column<string>(nullable: true),
                    PatentPublicDate = table.Column<DateTimeOffset>(nullable: true),
                    PatentPublicNumber = table.Column<string>(nullable: true),
                    PatentSrokEndDate = table.Column<DateTimeOffset>(nullable: true),
                    PatentType = table.Column<int>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationHistories", x => x.Id); });

            migrationBuilder.CreateTable(
                "IntegrationHistorySents",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationHistorySents", x => x.Id); });

            migrationBuilder.CreateTable(
                "IntegrationLogActions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionDate = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "GETDATE()"),
                    ActionTypeId = table.Column<int>(nullable: false),
                    BinListId = table.Column<int>(nullable: true),
                    DateFrom = table.Column<DateTimeOffset>(nullable: true),
                    DateTo = table.Column<DateTimeOffset>(nullable: true),
                    DigitalSignatureId = table.Column<int>(nullable: true),
                    HistoryId = table.Column<int>(nullable: true),
                    RnnListId = table.Column<int>(nullable: true),
                    SystemInfoAnswerId = table.Column<int>(nullable: true),
                    SystemInfoQueryId = table.Column<int>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationLogActions", x => x.Id); });

            migrationBuilder.CreateTable(
                "IntegrationLogDigitalSignatures",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Bin = table.Column<string>(nullable: true),
                    CertificateIsCorrect = table.Column<bool>(nullable: false),
                    CheckNote = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    EMail = table.Column<string>(nullable: true),
                    Iin = table.Column<string>(nullable: true),
                    PeriodFrom = table.Column<DateTimeOffset>(nullable: true),
                    PeriodTo = table.Column<DateTimeOffset>(nullable: true),
                    Person = table.Column<string>(nullable: true),
                    RawData = table.Column<byte[]>(nullable: true),
                    SignatureIsValid = table.Column<bool>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationLogDigitalSignatures", x => x.Id); });

            migrationBuilder.CreateTable(
                "IntegrationLogStrings",
                table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationLogStrings", x => new {x.Id, x.Index}); });

            migrationBuilder.CreateTable(
                "IntegrationLogSystemInfos",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    ChainId = table.Column<string>(nullable: true),
                    MessageDate = table.Column<DateTimeOffset>(nullable: true),
                    MessageId = table.Column<string>(nullable: true),
                    Sender = table.Column<string>(nullable: true),
                    StatusCode = table.Column<string>(nullable: true),
                    StatusMessageKz = table.Column<string>(nullable: true),
                    StatusMessageRu = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationLogSystemInfos", x => x.Id); });

            migrationBuilder.CreateTable(
                "IntegrationMonitorLogs",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    DbDateTime = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "GETDATE()"),
                    Error = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_IntegrationMonitorLogs", x => x.Id); });

            migrationBuilder.CreateTable(
                "LogActions",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    DbDateTime = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "GETDATE()"),
                    Note = table.Column<string>(nullable: true),
                    Project = table.Column<string>(nullable: true),
                    SystemInfoAnswerId = table.Column<int>(nullable: true),
                    SystemInfoQueryId = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_LogActions", x => x.Id); });

            migrationBuilder.CreateTable(
                "LogSystemInfos",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    ChainId = table.Column<string>(nullable: true),
                    DbDateTime = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "GETDATE()"),
                    MessageDate = table.Column<DateTimeOffset>(nullable: true),
                    MessageId = table.Column<string>(nullable: true),
                    Sender = table.Column<string>(nullable: true),
                    StatusCode = table.Column<string>(nullable: true),
                    StatusMessageKz = table.Column<string>(nullable: true),
                    StatusMessageRu = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_LogSystemInfos", x => x.Id); });

            migrationBuilder.CreateTable(
                "References",
                table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Ru = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_References", x => new {x.Id, x.Type}); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "IntegrationBulletins");

            migrationBuilder.DropTable(
                "IntegrationCalcCustomers");

            migrationBuilder.DropTable(
                "IntegrationCalcHistories");

            migrationBuilder.DropTable(
                "IntegrationCalcLinks");

            migrationBuilder.DropTable(
                "IntegrationCalcPatents");

            migrationBuilder.DropTable(
                "IntegrationHistories");

            migrationBuilder.DropTable(
                "IntegrationHistorySents");

            migrationBuilder.DropTable(
                "IntegrationLogActions");

            migrationBuilder.DropTable(
                "IntegrationLogDigitalSignatures");

            migrationBuilder.DropTable(
                "IntegrationLogStrings");

            migrationBuilder.DropTable(
                "IntegrationLogSystemInfos");

            migrationBuilder.DropTable(
                "IntegrationMonitorLogs");

            migrationBuilder.DropTable(
                "LogActions");

            migrationBuilder.DropTable(
                "LogSystemInfos");

            migrationBuilder.DropTable(
                "References");
        }
    }
}