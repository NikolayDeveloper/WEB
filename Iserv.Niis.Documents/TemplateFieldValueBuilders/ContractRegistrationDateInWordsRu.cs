using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Дата, месяц (прописью) и год регистрации договора на русском языке
    /// </summary>
    [TemplateFieldName(TemplateFieldName.ContractRegistrationDateInWordsRu)]
    internal class ContractRegistrationDateInWordsRu : TemplateFieldValueBase
    {
        public ContractRegistrationDateInWordsRu(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Дата, месяц (прописью) и год регистрации договора на русском языке
            return string.Empty;
        }
    }
}