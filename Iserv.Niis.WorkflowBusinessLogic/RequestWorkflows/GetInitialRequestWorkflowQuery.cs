using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.RequestWorkflows
{
    public class GetInitialRequestWorkflowQuery: BaseQuery
    {
        public RequestWorkflow Execute(Request request, int userId)
        {
            var dicProtectionDocTypesRepository = Uow.GetRepository<DicProtectionDocType>();
            var protectionDocType = dicProtectionDocTypesRepository.GetById(request.ProtectionDocTypeId);

            var dicRouteStagesRepository = Uow.GetRepository<DicRouteStage>();
            var initialStage = dicRouteStagesRepository.AsQueryable().FirstOrDefault(s => s.IsFirst && s.RouteId == protectionDocType.RouteId);

            if (initialStage == null)
            {
                return null;
            }

            var requestWorkflow = new RequestWorkflow
            {
                CurrentUserId = userId,
                OwnerId = request.Id,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
                IsMain = initialStage.IsMain,
            };

            return requestWorkflow;
        }
    }
}
