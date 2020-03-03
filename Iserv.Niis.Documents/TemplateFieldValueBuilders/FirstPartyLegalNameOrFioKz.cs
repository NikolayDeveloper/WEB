using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// наименование юридического лица / ФИО физического лица_кк
    /// </summary>
    [TemplateFieldName(TemplateFieldName.FirstPartyLegalNameOrFioKz)]
    internal class FirstPartyLegalNameOrFioKz : TemplateFieldValueBase
    {
        public FirstPartyLegalNameOrFioKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr наименование юридического лица / ФИО физического лица_кк
            return string.Empty;
        }
    }
}