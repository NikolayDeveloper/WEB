using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasMoreThanDocumentsByCodeRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] codes, int count = 0)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            var documents = request.Documents.Where(d => codes.Contains(d.Document.Type.Code));

            var isRequestHasMoreThanDocumentsByCode = documents.Count() > count;

            return isRequestHasMoreThanDocumentsByCode;
        }
    }
}
