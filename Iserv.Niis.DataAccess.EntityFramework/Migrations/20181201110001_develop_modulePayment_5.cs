using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class develop_modulePayment_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUses_AspNetUsers_CreateUserId",
                table: "PaymentUses");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUses_CreateUserId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "PaymentUses");

            migrationBuilder.AddColumn<decimal>(
                name: "BlockedAmount",
                table: "PaymentUses",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BlockedAmountEmployeeName",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlockedAmountReason",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "PaymentUses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfPayment",
                table: "PaymentUses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletionClearedPaymentDate",
                table: "PaymentUses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletionClearedPaymentEmployeeName",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletionClearedPaymentReason",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DicProtectionDocSubTypeId",
                table: "PaymentUses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DicProtectionDocTypeId",
                table: "PaymentUses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DicTariffId",
                table: "PaymentUses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCheckoutPaymentName",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeWriteOffPaymentName",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "IssuingPaymentDate",
                table: "PaymentUses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Payer",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerBinOrInn",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaymentUseDate",
                table: "PaymentUses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtectionDocId",
                table: "PaymentUses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "PaymentUses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnAmountEmployeeName",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnAmountReason",
                table: "PaymentUses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnedAmount",
                table: "PaymentUses",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReturnedAmountDate",
                table: "PaymentUses",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_ContractId",
                table: "PaymentUses",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_DicProtectionDocSubTypeId",
                table: "PaymentUses",
                column: "DicProtectionDocSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_DicProtectionDocTypeId",
                table: "PaymentUses",
                column: "DicProtectionDocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_DicTariffId",
                table: "PaymentUses",
                column: "DicTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_ProtectionDocId",
                table: "PaymentUses",
                column: "ProtectionDocId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_RequestId",
                table: "PaymentUses",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUses_Contracts_ContractId",
                table: "PaymentUses",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUses_DicProtectionDocSubTypes_DicProtectionDocSubTypeId",
                table: "PaymentUses",
                column: "DicProtectionDocSubTypeId",
                principalTable: "DicProtectionDocSubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUses_DicProtectionDocTypes_DicProtectionDocTypeId",
                table: "PaymentUses",
                column: "DicProtectionDocTypeId",
                principalTable: "DicProtectionDocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUses_DicTariffs_DicTariffId",
                table: "PaymentUses",
                column: "DicTariffId",
                principalTable: "DicTariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUses_ProtectionDocs_ProtectionDocId",
                table: "PaymentUses",
                column: "ProtectionDocId",
                principalTable: "ProtectionDocs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUses_Requests_RequestId",
                table: "PaymentUses",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUses_Contracts_ContractId",
                table: "PaymentUses");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUses_DicProtectionDocSubTypes_DicProtectionDocSubTypeId",
                table: "PaymentUses");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUses_DicProtectionDocTypes_DicProtectionDocTypeId",
                table: "PaymentUses");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUses_DicTariffs_DicTariffId",
                table: "PaymentUses");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUses_ProtectionDocs_ProtectionDocId",
                table: "PaymentUses");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentUses_Requests_RequestId",
                table: "PaymentUses");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUses_ContractId",
                table: "PaymentUses");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUses_DicProtectionDocSubTypeId",
                table: "PaymentUses");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUses_DicProtectionDocTypeId",
                table: "PaymentUses");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUses_DicTariffId",
                table: "PaymentUses");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUses_ProtectionDocId",
                table: "PaymentUses");

            migrationBuilder.DropIndex(
                name: "IX_PaymentUses_RequestId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "BlockedAmount",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "BlockedAmountEmployeeName",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "BlockedAmountReason",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "DateOfPayment",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "DeletionClearedPaymentDate",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "DeletionClearedPaymentEmployeeName",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "DeletionClearedPaymentReason",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "DicProtectionDocSubTypeId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "DicProtectionDocTypeId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "DicTariffId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "EmployeeCheckoutPaymentName",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "EmployeeWriteOffPaymentName",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "IssuingPaymentDate",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "Payer",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "PayerBinOrInn",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "PaymentUseDate",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "ProtectionDocId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "ReturnAmountEmployeeName",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "ReturnAmountReason",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "ReturnedAmount",
                table: "PaymentUses");

            migrationBuilder.DropColumn(
                name: "ReturnedAmountDate",
                table: "PaymentUses");

            migrationBuilder.AddColumn<int>(
                name: "CreateUserId",
                table: "PaymentUses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUses_CreateUserId",
                table: "PaymentUses",
                column: "CreateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentUses_AspNetUsers_CreateUserId",
                table: "PaymentUses",
                column: "CreateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
