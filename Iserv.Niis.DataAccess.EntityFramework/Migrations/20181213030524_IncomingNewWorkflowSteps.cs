using Microsoft.EntityFrameworkCore.Migrations;

namespace Iserv.Niis.DataAccess.EntityFramework.Migrations
{
    public partial class IncomingNewWorkflowSteps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql(@"insert into [DicRouteStages] (Code, NameRu, DateCreate, DateUpdate, Interval, IsFirst, IsLast, IsMultiUser, IsReturnable, IsSystem, RouteId, IsAuto, ExpirationType, IsMain)
	           //                    values (N'IN1.2.3', N'Контроль начальника департамента', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 24, 0, 0, 1, 1, 0, 1, 0, 0, 1),
	           //                           (N'IN2.2.4', N'Контроль начальника управления', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 24, 0, 0, 0, 1, 0, 1, 0, 0, 1)");

            //migrationBuilder.Sql(@"insert into RouteStageOrders (CurrentStageId, DateCreate, DateUpdate, IsAutomatic, IsParallel, IsReturn, NextStageId) 
            //                       values ((select top 1 id from [DicRouteStages] where Code = 'IN1.2.2'), SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, 0, 0, (select top 1 id from [DicRouteStages] where Code = 'IN1.2.3')),
            //                              ((select top 1 id from [DicRouteStages] where Code = 'IN1.2.3'), SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, 0, 0, (select top 1 id from [DicRouteStages] where Code = 'IN2.1')),
            //                              ((select top 1 id from [DicRouteStages] where Code = 'IN1.2.3'), SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, 0, 0, (select top 1 id from [DicRouteStages] where Code = 'IN2.2.4')),
            //                              ((select top 1 id from [DicRouteStages] where Code = 'IN2.2.4'), SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, 0, 0, (select top 1 id from [DicRouteStages] where Code = 'IN2.1')),
            //                              ((select top 1 id from [DicRouteStages] where Code = 'IN2.2.4'), SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 0, 0, 0, (select top 1 id from [DicRouteStages] where Code = 'IN2.2')) ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
