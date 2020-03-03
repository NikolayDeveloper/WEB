using System;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.RequestWorkflows
{
    public class GetLastRequestWorkflowByRequestIdStageCodeQuery : BaseQuery
    {
        public RequestWorkflow Execute(int requestId, string stageCode)
        {
            if (string.IsNullOrWhiteSpace(stageCode))
                throw new ArgumentException(nameof(stageCode));

            var requestWorkflow = Uow.GetRepository<RequestWorkflow>()
                .AsQueryable()
                .Include(i => i.CurrentStage)
                .LastOrDefault(i => i.OwnerId == requestId && stageCode.Contains(i.CurrentStage.Code));

            return requestWorkflow;
        }
    }
}