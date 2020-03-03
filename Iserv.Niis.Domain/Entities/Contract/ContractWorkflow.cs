using Iserv.Niis.Domain.Entities.Workflow;

namespace Iserv.Niis.Domain.Entities.Contract
{
    public class ContractWorkflow : BaseWorkflow
    {
        public int OwnerId { get; set; }
        public Contract Owner { get; set; }
    }
}