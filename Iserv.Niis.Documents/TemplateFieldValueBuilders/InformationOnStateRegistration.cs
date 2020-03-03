using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Сведения о гос.регистрации
    /// </summary>
    [TemplateFieldName(TemplateFieldName.InformationOnStateRegistration)]
    internal class InformationOnStateRegistration : TemplateFieldValueBase
    {
        public InformationOnStateRegistration(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Сведения о гос.регистрации
            return string.Empty;
        }
    }
}