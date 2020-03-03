using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Дата, месяц (прописью), год продления срока действия патента, на русском языке
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PatentRenewalDateInWord)]
    internal class PatentRenewalDateInWord : TemplateFieldValueBase
    {
        public PatentRenewalDateInWord(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Дата, месяц (прописью), год продления срока действия патента, на русском языке
            return string.Empty;
        }
    }
}