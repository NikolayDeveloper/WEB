using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Дата, месяц (прописью), год о восстановления патента, на русском языке
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PatentRestorationDateInWord)]
    internal class PatentRestorationDateInWord : TemplateFieldValueBase
    {
        public PatentRestorationDateInWord(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Дата, месяц (прописью), год о восстановления патента, на русском языке
            return string.Empty;
        }
    }
}