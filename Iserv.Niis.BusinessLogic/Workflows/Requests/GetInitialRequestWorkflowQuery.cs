using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Workflows.Requests
{
    public class GetInitialRequestWorkflowQuery : BaseQuery
    {
        public async Task<RequestWorkflow> ExecuteAsync(Request request, int userId)
        {
            var dicProtectionDocTypesRepository = Uow.GetRepository<DicProtectionDocType>();
            var protectionDocType = await dicProtectionDocTypesRepository.AsQueryable().FirstOrDefaultAsync(t => t.Id == request.ProtectionDocTypeId);

            var dicRouteStagesRepository = Uow.GetRepository<DicRouteStage>();
            var initialStage = await dicRouteStagesRepository.AsQueryable().FirstOrDefaultAsync(s => !s.IsDeleted && s.IsFirst && s.RouteId == protectionDocType.RouteId);

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