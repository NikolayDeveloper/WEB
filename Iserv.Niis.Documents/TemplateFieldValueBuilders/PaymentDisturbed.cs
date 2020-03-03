using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Model.Models.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Распределение платежа.
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PaymentDisturbed)]
    public class PaymentDisturbed : TemplateFieldValueBase
    {
        public PaymentDisturbed(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "Payment" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            PaymentDto payment = parameters["Payment"] as PaymentDto;

            if (payment is null)
            {
                return string.Empty;
            }

            return payment.PaymentUseAmountSum.ToString();
        }

       
    }
}
