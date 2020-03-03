using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class PaymentAddReturnedAmountField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ReturnedAmount",
                table: "Payments",
                type: "decimal(18, 2)",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "SearchContractsView",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        AuthorNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AuthorsAreNotMentions = table.Column<bool>(type: "bit", nullable: false),
            //        Barcode = table.Column<int>(type: "int", nullable: false),
            //        BreedingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConfidantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CorrespondenceNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        DeclarantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DisclaimerKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DisclaimerRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DocTypeId = table.Column<int>(type: "int", nullable: true),
            //        DocTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcfemCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcgsCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcisCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
            //        IncomingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IpcCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NumberBulletin = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PatentAttorneyNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PatentOwnerNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocExtensionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocMaintainYear = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocOutgoingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocStatusId = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocValidDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
            //        ReceiveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestStatusId = table.Column<int>(type: "int", nullable: true),
            //        RequestStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestSubTypeId = table.Column<int>(type: "int", nullable: true),
            //        RequestSubTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestTypeId = table.Column<int>(type: "int", nullable: true),
            //        RequestTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SelectionAchieveTypeId = table.Column<int>(type: "int", nullable: true),
            //        SelectionAchieveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SearchContractsView", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SearchProtectionDocsView",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        AuthorNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AuthorsAreNotMentions = table.Column<bool>(type: "bit", nullable: false),
            //        Barcode = table.Column<int>(type: "int", nullable: false),
            //        BreedingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConfidantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CorrespondenceNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        DeclarantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DisclaimerKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DisclaimerRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DocTypeId = table.Column<int>(type: "int", nullable: true),
            //        DocTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcfemCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcgsCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcisCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
            //        IncomingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IpcCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NumberBulletin = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PatentAttorneyNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PatentOwnerNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocExtensionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocMaintainYear = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocOutgoingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocStatusId = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocValidDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
            //        ReceiveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestStatusId = table.Column<int>(type: "int", nullable: true),
            //        RequestStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestSubTypeId = table.Column<int>(type: "int", nullable: true),
            //        RequestSubTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestTypeId = table.Column<int>(type: "int", nullable: true),
            //        RequestTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SelectionAchieveTypeId = table.Column<int>(type: "int", nullable: true),
            //        SelectionAchieveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SearchProtectionDocsView", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SearchRequestsView",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        AuthorNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AuthorsAreNotMentions = table.Column<bool>(type: "bit", nullable: false),
            //        Barcode = table.Column<int>(type: "int", nullable: false),
            //        BreedingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConfidantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CorrespondenceNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DateCreate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        DeclarantNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DisclaimerKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DisclaimerRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DocTypeId = table.Column<int>(type: "int", nullable: true),
            //        DocTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcfemCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcgsCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IcisCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
            //        IncomingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IpcCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameKz = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NameRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NumberBulletin = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PatentAttorneyNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PatentOwnerNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocExtensionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocMaintainYear = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocOutgoingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ProtectionDocStatusId = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocTypeId = table.Column<int>(type: "int", nullable: true),
            //        ProtectionDocTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ProtectionDocValidDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        ReceiveTypeId = table.Column<int>(type: "int", nullable: true),
            //        ReceiveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RegDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        RegNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestStatusId = table.Column<int>(type: "int", nullable: true),
            //        RequestStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestSubTypeId = table.Column<int>(type: "int", nullable: true),
            //        RequestSubTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RequestTypeId = table.Column<int>(type: "int", nullable: true),
            //        RequestTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SelectionAchieveTypeId = table.Column<int>(type: "int", nullable: true),
            //        SelectionAchieveTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SearchRequestsView", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "SearchContractsView");

            //migrationBuilder.DropTable(
            //    name: "SearchProtectionDocsView");

            //migrationBuilder.DropTable(
            //    name: "SearchRequestsView");
            //    name: "SearchRequestsView");

            migrationBuilder.DropColumn(
                name: "ReturnedAmount",
                table: "Payments");
        }
    }
}
