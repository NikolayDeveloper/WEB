using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using System.Collections.Generic;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument
{
    public class ProtectionDocumentWorkFlowRequest : WorkflowRequest<ProtectionDoc>
    {
        public ProtectionDocumentWorkFlowRequest()
        {
            this.SpecificNextStageUserIds = new Dictionary<string, int>();
        }
        public int ProtectionDocId { get; set; }
        public int DocumentId { get; set; }
        public int NextStageUserId { get; set; }
        public Dictionary<string, int> SpecificNextStageUserIds { get; set; }
        public string NextStageCode { get; set; }
        public int? NextStageSecondaryUserId { get; set; }
    }
}
