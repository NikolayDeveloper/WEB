using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Payment
{
    public class PaymentCharge: Entity<int>
    {
        public int TariffId { get; set; }
        public DicTariff Tariff { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int WorkflowId { get; set; }
        public RequestWorkflow Workflow { get; set; }
    }
}
