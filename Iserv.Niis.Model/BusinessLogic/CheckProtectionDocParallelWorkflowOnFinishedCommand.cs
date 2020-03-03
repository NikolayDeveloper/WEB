using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
//using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Model.BusinessLogic
{
    public class CheckProtectionDocParallelWorkflowOnFinishedCommand : BaseQuery
    {
        public bool Execute(int ownerID)
        {
            var requestParallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflows = requestParallelWorkflowRepository.AsQueryable();

            return !parallelWorkflows
                   .Any(x => x.OwnerId == ownerID && !x.IsFinished);
        }
    }
}
