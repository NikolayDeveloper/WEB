using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.Payment
{
    public class PaymentListDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string PaymentNumber { get; set; }
        public decimal? Amount { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public string Payment1CNumber { get; set; }
        public bool? IsPrePayment { get; set; }
        public string PurposeDescription { get; set; }
        public string AssignmentDescription { get; set; }
        public decimal? CurrencyAmount { get; set; }
        public decimal? CurrencyRate { get; set; }
        public string CurrencyType { get; set; }
        public string CustomerXin { set; get; }
        public string CustomerNameRu { set; get; }
        public decimal? ReturnedAmount { get; set; }
        public decimal? BlockedAmount { get; set; }
        /// <summary>
        /// Платёж в иностранной валюте
        /// </summary>
        public bool IsForeignCurrency { get; set; }
    }
}
