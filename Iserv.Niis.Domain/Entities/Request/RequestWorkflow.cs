using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.Workflow;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class RequestWorkflow : BaseWorkflow
    {
        public RequestWorkflow()
        {
            PaymentCharges = new HashSet<PaymentCharge>();
        }
        public int OwnerId { get; set; }
        public Entities.Request.Request Owner { get; set; }
        public bool? IsChangeScenarioEntry { get; set; }
        public ICollection<PaymentCharge> PaymentCharges { get; set; }
    }
}