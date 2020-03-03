using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Лицо, которому предоставили права_Сторона 2, каз
    /// </summary>
    [TemplateFieldName(TemplateFieldName.SecondPartyRightOwnerKz)]
    internal class SecondPartyRightOwnerKz : TemplateFieldValueBase
    {
        public SecondPartyRightOwnerKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Лицо, которому предоставили права_Сторона 2, каз
            return string.Empty;
        }
    }
}