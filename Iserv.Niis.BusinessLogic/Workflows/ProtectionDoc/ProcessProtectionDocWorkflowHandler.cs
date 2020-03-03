using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc
{
    public class ProcessProtectionDocWorkflowHandler: BaseHandler
    {
        public async Task Handle(ProtectionDocWorkflow protectionDocWorkflow, Domain.Entities.ProtectionDoc.ProtectionDoc protectionDoc)
        {
            var routeStage = protectionDocWorkflow.CurrentStageId.HasValue
                ? Executor.GetQuery<GetDicRouteStageByIdQuery>().Process(q => q.Execute(protectionDocWorkflow.CurrentStageId.Value))
                : null;

            protectionDoc.CurrentWorkflow = protectionDocWorkflow;
            protectionDoc.StatusId = routeStage?.ProtectionDocStatusId ?? protectionDoc.StatusId;

            await Executor.GetCommand<UpdateProtectionDocCommand>().Process(c => c.ExecuteAsync(protectionDoc.Id, protectionDoc));
        }
    }
}
