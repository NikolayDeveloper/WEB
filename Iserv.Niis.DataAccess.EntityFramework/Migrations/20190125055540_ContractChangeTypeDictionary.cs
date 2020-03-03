using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class ContractChangeTypeDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Contracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TypeId",
                table: "Contracts",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_DicContractTypes_TypeId",
                table: "Contracts",
                column: "TypeId",
                principalTable: "DicContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_DicContractTypes_TypeId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_TypeId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Contracts");
        }
    }
}
