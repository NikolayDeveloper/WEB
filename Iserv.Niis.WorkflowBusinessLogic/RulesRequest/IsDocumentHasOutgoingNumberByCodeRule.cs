using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;
using System.Linq;
using Iserv.Niis.WorkflowBusinessLogic.Requests;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsDocumentHasOutgoingNumberByCodeRule : BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string[] typeCodes)
        {
            var documents = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>()
                .Process(q => q.Execute(WorkflowRequest.RequestId, typeCodes));
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            if (request == null)
            {
                return false;
            }

            var isHasDocumentWithOutgoingNumber = documents.Any(d => d.CurrentWorkflows.All(cwf => cwf.DateCreate > request.CurrentWorkflow.DateCreate) &&
                !string.IsNullOrEmpty(d.OutgoingNumber));

            return isHasDocumentWithOutgoingNumber;
        }

        public bool Eval(string typeCode)
        {
            var documents = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>()
                .Process(q => q.Execute(WorkflowRequest.RequestId, new[] { typeCode }));
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            if (request == null)
            {
                return false;
            }

            var isHasDocumentWithOutgoingNumber = documents.Any(d => d.CurrentWorkflows.All(cwf => cwf.DateCreate > request.CurrentWorkflow.DateCreate) &&
                !string.IsNullOrEmpty(d.OutgoingNumber));

            return isHasDocumentWithOutgoingNumber;
        }

    }
}
