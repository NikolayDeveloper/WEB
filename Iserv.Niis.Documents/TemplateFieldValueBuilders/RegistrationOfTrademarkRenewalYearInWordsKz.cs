using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Год продления (на государственном языке)
    /// </summary>
    [TemplateFieldName(TemplateFieldName.RegistrationOfTrademarkRenewalYearInWordsKz)]
    internal class RegistrationOfTrademarkRenewalYearInWordsKz : TemplateFieldValueBase
    {
        public RegistrationOfTrademarkRenewalYearInWordsKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Год продления (на государственном языке)
            return string.Empty;
        }
    }
}