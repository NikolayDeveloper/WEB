
namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowDocuments
{
    public class DocumentWorkFlowRequest : WorkflowRequest<Domain.Entities.Document.Document>
    {
        public int DocumentId { get; set; }


        public string PrevStageCode { get; set; }
        public int NextStageUserId { get; set; }
        public string NextStageCode { get; set; }
    }
}
