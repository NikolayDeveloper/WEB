using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows.Contracts
{
    public class GetInitialContractWorkflowQuery : BaseQuery
    {
        public async Task<ContractWorkflow> ExecuteAsync(Contract contract, int userId)
        {
            var dicProtectionDocTypesRepository = Uow.GetRepository<DicProtectionDocType>();
            var protectionDocType = await dicProtectionDocTypesRepository.AsQueryable().FirstOrDefaultAsync(t => t.Id == contract.ProtectionDocTypeId);

            var dicRouteStagesRepository = Uow.GetRepository<DicRouteStage>();
            var initialStage = await dicRouteStagesRepository.AsQueryable().FirstOrDefaultAsync(s => s.IsFirst && s.RouteId == protectionDocType.RouteId);

            if (initialStage == null)
            {
                return null;
            }

            var contractWorkflow = new ContractWorkflow
            {
                CurrentUserId = userId,
                OwnerId = contract.Id,
                CurrentStageId = initialStage.Id,
                RouteId = initialStage.RouteId,
                IsComplete = initialStage.IsLast,
                IsSystem = initialStage.IsSystem,
            };

            return contractWorkflow;
        }
    }
}
