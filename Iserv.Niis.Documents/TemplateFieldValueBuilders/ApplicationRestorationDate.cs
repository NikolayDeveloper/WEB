using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    ///     Крайний срок восстановления заявки
    /// </summary>
    [TemplateFieldName(TemplateFieldName.ApplicationRestorationDate)]
    internal class ApplicationRestorationDate : TemplateFieldValueBase
    {
        public ApplicationRestorationDate(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Крайний срок восстановления заявки
            return string.Empty;
        }
    }
}