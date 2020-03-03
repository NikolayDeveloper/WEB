using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Дата, месяц (прописью: қаңтарынан, ақпанынан, наурызынан, сәуiрінен, мамырынан, маусымынан, шiлдесінен, тамызынан, қыркүйегінен, қазанынан, қарашасынан‚ желтоқсанынан) о восстановлении патента, на государственном языке
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PatentRestorationDateInWordKz)]
    internal class PatentRestorationDateInWordKz : TemplateFieldValueBase
    {
        public PatentRestorationDateInWordKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Дата, месяц (прописью: қаңтарынан, ақпанынан, наурызынан, сәуiрінен, мамырынан, маусымынан, шiлдесінен, тамызынан, қыркүйегінен, қазанынан, қарашасынан‚ желтоқсанынан) о восстановлении патента, на государственном языке
            return string.Empty;
        }
    }
}