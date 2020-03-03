using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Model.Models.PaymentsJournal;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Наименование услуги.
    /// </summary>
    [TemplateFieldName(TemplateFieldName.ServiceName)]
    public class ServiceName : TemplateFieldValueBase
    {
        public ServiceName(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"PaymentUse"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            PaymentUseDto paymentUseDto = parameters["PaymentUse"] as PaymentUseDto;
            
            if (paymentUseDto is null)
            {
                return string.Empty;
            }

            return paymentUseDto.ServiceName ?? string.Empty;
        }
    }
}
