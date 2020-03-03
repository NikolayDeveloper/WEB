using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Payment
{
    public class PaymentExecutor: Entity<int>
    {
        public int RequestId { get; set; }
        public Request.Request Request { get; set; }
        public int TariffId { get; set; }
        public DicTariff Tariff { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool IsCharged { get; set; }
    }
}
