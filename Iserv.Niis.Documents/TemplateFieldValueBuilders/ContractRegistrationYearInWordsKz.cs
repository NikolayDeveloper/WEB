using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Год регистрации договора (на государственном языке)
    /// </summary>
    [TemplateFieldName(TemplateFieldName.ContractRegistrationYearInWordsKz)]
    internal class ContractRegistrationYearInWordsKz : TemplateFieldValueBase
    {
        public ContractRegistrationYearInWordsKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Год регистрации договора (на государственном языке)
            return string.Empty;
        }
    }
}