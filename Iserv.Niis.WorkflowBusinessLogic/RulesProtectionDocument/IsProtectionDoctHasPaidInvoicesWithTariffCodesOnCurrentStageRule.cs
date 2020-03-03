using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Payment;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesProtectionDocument
{
    public class IsProtectionDoctHasPaidInvoicesWithTariffCodesOnCurrentStageRule: BaseRule<ProtectionDocumentWorkFlowRequest>
    {
        public bool Eval(string[] tariffCodes)
        {
            var paymentInvoices = Executor.GetQuery<GetPaymentInvoicesByProtectionDocIdAndTariffCodesQuery>()
                .Process(r => r.Execute(WorkflowRequest.ProtectionDocId, tariffCodes));

            if (!paymentInvoices.Any())
                return false;

            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute(WorkflowRequest.ProtectionDocId));
            if (protectionDoc == null)
            {
                return false;
            }

            return paymentInvoices
                .Any(i => new[] { DicPaymentStatusCodes.Credited, DicPaymentStatusCodes.Charged }.Contains(i.Status.Code) && i.DateCreate > protectionDoc.CurrentWorkflow.DateCreate);
        }
    }
}
