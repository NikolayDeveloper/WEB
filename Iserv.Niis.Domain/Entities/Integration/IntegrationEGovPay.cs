using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Domain.Entities.Integration
{
    public class IntegrationEGovPay
    {
        public int Id { get; set; }
        public int RequestBarcode { get; set; }
        public string PayCode { get; set; }
        public decimal? PaySum { get; set; }
        public DateTimeOffset? PayDate { get; set; }
        public string PayXin { get; set; }
        public string PayXml { get; set; }
    }
}
