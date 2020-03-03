using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class DictionaryEntitySoftDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicTypeTrademarks",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicTypeTrademarks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicTariffs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicTariffs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicSendTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicSendTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicSelectionAchieveTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicSelectionAchieveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicRouteStages",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicRouteStages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicRoutes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicRoutes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicRequisitionFeedTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicRequisitionFeedTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicRequestStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicRequestStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicRedefinitionTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicRedefinitionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicRedefinitionDocumentTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicRedefinitionDocumentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicReceiveTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicReceiveTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicProtectionDocTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicProtectionDocTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicProtectionDocTMTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicProtectionDocTMTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicProtectionDocSubTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicProtectionDocSubTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicProtectionDocStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicProtectionDocStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicProtectionDocBulletinTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicProtectionDocBulletinTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicPositions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicPositions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicPaymentStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicPaymentStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicOnlineRequisitionStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicOnlineRequisitionStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicNotificationStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicNotificationStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicLogTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicLogTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicLocationTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicLocationTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicLocations",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicLocations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicIPCs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicIPCs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicIntellectualPropertyStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicIntellectualPropertyStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicICISs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicICISs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicICGSs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicICGSs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicICFEMs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicICFEMs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicEventTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicEventTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicEntityAccessTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicEntityAccessTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicEarlyRegTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicEarlyRegTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicDocumentTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicDocumentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicDocumentStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicDocumentStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicDocumentClassifications",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicDocumentClassifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicDivisions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicDivisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicDetailICGSs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicDetailICGSs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicDepartmentTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicDepartmentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicDepartments",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicDepartments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicCustomerTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicCustomerTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicCustomers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicCustomerRoles",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicCustomerRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicCountries",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicCountries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicConventionTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicConventionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicContractTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicContractTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicContractStatuses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicContractStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicContractCategories",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicContractCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicContinents",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicContinents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicContactInfoTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicContactInfoTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicConsiderationTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicConsiderationTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicColorTZs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicColorTZs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicBeneficiaryTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicBeneficiaryTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicApplicantTypes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicApplicantTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "DicAddresses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DicAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicTypeTrademarks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicTypeTrademarks");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicTariffs");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicSendTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicSendTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicSelectionAchieveTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicSelectionAchieveTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicRouteStages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicRouteStages");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicRoutes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicRoutes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicRequisitionFeedTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicRequisitionFeedTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicRequestStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicRequestStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicRedefinitionTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicRedefinitionTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicRedefinitionDocumentTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicRedefinitionDocumentTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicReceiveTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicReceiveTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicProtectionDocTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicProtectionDocTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicProtectionDocTMTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicProtectionDocTMTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicProtectionDocSubTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicProtectionDocSubTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicProtectionDocStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicProtectionDocStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicProtectionDocBulletinTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicProtectionDocBulletinTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicPositions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicPositions");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicPaymentStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicPaymentStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicOnlineRequisitionStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicOnlineRequisitionStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicNotificationStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicNotificationStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicLogTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicLogTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicLocationTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicLocationTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicLocations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicLocations");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicIPCs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicIPCs");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicIntellectualPropertyStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicIntellectualPropertyStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicICISs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicICISs");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicICGSs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicICGSs");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicICFEMs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicICFEMs");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicEventTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicEventTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicEntityAccessTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicEntityAccessTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicEarlyRegTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicEarlyRegTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicDocumentTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicDocumentTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicDocumentStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicDocumentStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicDocumentClassifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicDocumentClassifications");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicDivisions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicDivisions");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicDetailICGSs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicDetailICGSs");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicDepartmentTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicDepartmentTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicDepartments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicDepartments");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicCustomerTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicCustomerTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicCustomers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicCustomers");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicCustomerRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicCustomerRoles");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicCountries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicCountries");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicConventionTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicConventionTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicContractTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicContractTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicContractStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicContractStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicContractCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicContractCategories");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicContinents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicContinents");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicContactInfoTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicContactInfoTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicConsiderationTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicConsiderationTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicColorTZs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicColorTZs");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicBeneficiaryTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicBeneficiaryTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicApplicantTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicApplicantTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DicAddresses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DicAddresses");
        }
    }
}
