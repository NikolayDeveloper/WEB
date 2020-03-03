using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Данные по предоставленному праву использования товарного знака (лицо и адрес) на русском языке, Сторона 2
    /// </summary>
    [TemplateFieldName(TemplateFieldName.TrademarkUseRighsRu)]
    internal class TrademarkUseRighsRu : TemplateFieldValueBase
    {
        public TrademarkUseRighsRu(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Данные по предоставленному праву использования товарного знака (лицо и адрес) на русском языке, Сторона 2
            return string.Empty;
        }
    }
}