using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class RemoveOldContractTypeRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
   //         migrationBuilder.Sql(@"
   //             INSERT INTO dbo.DicCustomerRoles(Code, DateCreate, DateUpdate, NameRu, IsDeleted) VALUES 
   //             (N'9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Залогодатель', 0),
   //             (N'10', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Залогодержатель', 0),
   //             (N'11', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Правопреемник', 0),
   //             (N'12', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Сублицензиат', 0)

   //             INSERT INTO dbo.DicContractTypes(Code, NameRu, StageOneId, StageTwoId, DateCreate, DateUpdate, IsDeleted)
   //             SELECT dpdst.Code, dpdst.NameRu, (SELECT TOP 1 id FROM dbo.DicCustomerRoles WHERE LOWER(NameRu) = LOWER(dpdst.S1)), (SELECT TOP 1 id FROM dbo.DicCustomerRoles WHERE LOWER(NameRu) = LOWER(dpdst.S2)), SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0 FROM dbo.DicProtectionDocSubTypes dpdst WHERE TypeId = 72
				
			//	UPDATE dbo.Contracts SET TypeId = (SELECT TOP 1 id FROM dbo.DicContractTypes WHERE Code = (SELECT TOP 1 Code FROM dbo.DicProtectionDocSubTypes WHERE id = ContractTypeId))
			//");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_DicProtectionDocSubTypes_ContractTypeId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractTypeId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractTypeId",
                table: "Contracts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractTypeId",
                table: "Contracts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractTypeId",
                table: "Contracts",
                column: "ContractTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_DicProtectionDocSubTypes_ContractTypeId",
                table: "Contracts",
                column: "ContractTypeId",
                principalTable: "DicProtectionDocSubTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
