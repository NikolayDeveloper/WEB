using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasDocumentWithCodeCreatedOnCurrentStageRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(params string[] typeCodes)
        {
            var documents = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.RequestId, typeCodes));
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            if (request == null)
            {
                return false;
            }
            return documents.Any(d => d.DateCreate > request.CurrentWorkflow.DateCreate);
        }
    }
}
