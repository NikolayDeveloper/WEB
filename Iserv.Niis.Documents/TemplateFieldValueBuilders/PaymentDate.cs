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
    /// Дата платежа.
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PaymentDate)]
    public class PaymentDate : TemplateFieldValueBase
    {
        public PaymentDate(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"PaymentUse"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            PaymentUseDto paymentUse = parameters["PaymentUse"] as PaymentUseDto;

            if (paymentUse is null)
            {
                return string.Empty;
            }

            return paymentUse.PaymentDate.ToTemplateDateFormat();
        }
    }
}
