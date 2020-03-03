using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests
{
    public class RequestWorkFlowRequest : WorkflowRequest<Request>
    {
        public int RequestId { get; set; }
        public int DocumentId { get; set; }
        public int? PaymentInvoiceId { get; set; }
        public int NextStageUserId { get; set; }

        public string PrevStageCode { get; set; }
        public string NextStageCode { get; set; }     
    }
}
