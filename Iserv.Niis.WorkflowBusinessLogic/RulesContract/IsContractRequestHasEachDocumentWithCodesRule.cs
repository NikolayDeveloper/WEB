using Iserv.Niis.WorkflowBusinessLogic.Contracts;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using NetCoreRules;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesContract
{
    public class IsContractRequestHasEachDocumentWithCodesRule : BaseRule<ContractWorkFlowRequest>
    {
        public bool Eval(string[] typeCodes)
        {
            var contract = Executor.GetQuery<GetContractByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.ContractId));

            var documentIds = contract.Documents
                .Where(d => typeCodes.Contains(d.Document.Type.Code))
                .Select(d => d.DocumentId)
                .ToList();

            var requestIdsBelongToContract = Executor.GetQuery<GetRequestIdsByContractIdQuery>().Process(q => q.Execute(contract.Id));
            var requestIdsBelongToDocuments = Executor.GetQuery<GetRequestIdsByDocumentIdsQuery>().Process(q => q.Execute(documentIds));
            var isContractHasEachDocumentForRequest = requestIdsBelongToContract.Except(requestIdsBelongToDocuments).Any() == false
               && documentIds.Count >= requestIdsBelongToContract.Count;

            var protectionDocIdsBelongToContract = Executor.GetQuery<GetProtectionDocIdsByContractIdQuery>().Process(q => q.Execute(contract.Id));
            var protectionDocIdsBelongToDocuments = Executor.GetQuery<GetProtectionDocIdsByDocumentIdsQuery>().Process(q => q.Execute(documentIds));
            var isContractHasEachDocumentForProtectionDoc = protectionDocIdsBelongToContract.Except(protectionDocIdsBelongToDocuments).Any() == false
                && documentIds.Count >= protectionDocIdsBelongToContract.Count;

            return isContractHasEachDocumentForRequest && isContractHasEachDocumentForProtectionDoc;
        }
    }
}
