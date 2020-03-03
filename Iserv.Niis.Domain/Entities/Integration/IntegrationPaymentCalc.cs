using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationPaymentCalc
    {
        public int Id { get; set; }
        public int CorId { get; set; }
        public int PatentType { get; set; }
        public int? MinCount { get; set; }
        public int TariffId { get; set; }
        public string CountName { get; set; }
    }
}
