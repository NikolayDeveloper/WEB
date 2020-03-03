using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc
{
    public class GetInitialProtectionDocWorkflowQuery: BaseQuery
    {
        public async Task<ProtectionDocWorkflow> ExecuteAsync(int protectionDocId, int userId)
        {
            var dicRouteStagesRepository = Uow.GetRepository<DicRouteStage>();
            var initialStage = await dicRouteStagesRepository.AsQueryable().FirstOrDefaultAsync(s => s.IsFirst && s.RouteId == 22);

            if (initialStage == null)
            {
                return null;
            }

            var protectionDocWorkflow = new ProtectionDocWorkflow
            {
                CurrentUserId = userId,
                OwnerId = protectionDocId,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain,
            };

            return protectionDocWorkflow;
        }
    }
}
