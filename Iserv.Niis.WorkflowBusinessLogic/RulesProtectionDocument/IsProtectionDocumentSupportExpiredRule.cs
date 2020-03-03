using Iserv.Niis.DI;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument
{
    public class IsProtectionDocumentSupportExpiredRule: BaseRule<ProtectionDocumentWorkFlowRequest>
    {
        public bool Eval()
        {
            var protectonDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                .Process(q => q.Execute(WorkflowRequest.ProtectionDocId));

            return NiisAmbientContext.Current.DateTimeProvider.Now > protectonDoc.MaintainDate;
        }
    }
}
