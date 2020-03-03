using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.Model.Models.PaymentsJournal;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Платеж авансовый.
    /// </summary>
    [TemplateFieldName(TemplateFieldName.IsAdvancePayment)]
    public class IsAdvancePayment : TemplateFieldValueBase
    {
        public IsAdvancePayment(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"Payment"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            PaymentUseDto payment = parameters["PaymentUse"] as PaymentUseDto;

            if (payment is null)
            {
                return string.Empty;
            }

            return payment.IsAdvancePayment.ToTemplateFormat();
        }
    }
}
