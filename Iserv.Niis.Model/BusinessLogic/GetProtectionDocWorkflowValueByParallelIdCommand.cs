using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Model.BusinessLogic
{
    public class GetProtectionDocWorkflowValueByParallelIdCommand : BaseQuery
    {
        public string Execute(int ownerID)
        {
            var requestParallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflows = requestParallelWorkflowRepository.AsQueryable();

            var requestProtectionDocWorkflow = Uow.GetRepository<ProtectionDocWorkflow>();

            var appRequestParallelWorkflowIds = parallelWorkflows
                .Where(x => x.OwnerId == ownerID && !x.IsFinished)
                .Select(x => x.ProtectionDocWorkflowId).ToArray();
            var protectionDocWorkflows = requestProtectionDocWorkflow
                .AsQueryable()
                .Where(x => appRequestParallelWorkflowIds.Contains(x.Id) && !string.IsNullOrEmpty(x.CurrentStage.NameRu))
                .Select(x => x.CurrentStage.NameRu);

            return string.Join(" | ", protectionDocWorkflows.ToList());
        }
    }
}
