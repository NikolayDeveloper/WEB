using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Дата, месяц (прописью: қаңтарына, ақпанына, наурызына, сәуiріне, мамырына, маусымына, шiлдесіне, тамызына, қыркүйегіне, қазанына, қарашасына‚ желтоқсанына) продления срока действия, на государственном языке
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PatentRenewalDateInWordKz)]
    internal class PatentRenewalDateInWordKz : TemplateFieldValueBase
    {
        public PatentRenewalDateInWordKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Дата, месяц (прописью: қаңтарына, ақпанына, наурызына, сәуiріне, мамырына, маусымына, шiлдесіне, тамызына, қыркүйегіне, қазанына, қарашасына‚ желтоқсанына) продления срока действия, на государственном языке
            return string.Empty;
        }
    }
}