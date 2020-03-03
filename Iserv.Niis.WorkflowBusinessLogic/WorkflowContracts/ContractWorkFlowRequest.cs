using Iserv.Niis.Domain.Entities.Contract;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts
{
    public class ContractWorkFlowRequest : WorkflowRequest<Contract>
    {
        public int ContractId { get; set; }
        public int DocumentId { get; set; }
        public int NextStageUserId { get; set; }
        public string NextStageCode { get; set; }
    }
}
