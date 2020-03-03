using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Дата и месяц (прописью: қаңтары‚ ақпаны‚ наурызы‚ сәуiрі‚ мамыры‚ маусымы‚ шiлдесі‚ тамызы‚ қыркүйегі, қазаны‚ қарашасы‚ желтоқсаны) регистрации договора на государственном языке
    /// </summary>
    [TemplateFieldName(TemplateFieldName.ContractRegistrationDateInWordsKz)]
    internal class ContractRegistrationDateInWordsKz : TemplateFieldValueBase
    {
        public ContractRegistrationDateInWordsKz(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            // TODO Z.Alexandr Дата и месяц (прописью: қаңтары‚ ақпаны‚ наурызы‚ сәуiрі‚ мамыры‚ маусымы‚ шiлдесі‚ тамызы‚ қыркүйегі, қазаны‚ қарашасы‚ желтоқсаны) регистрации договора на государственном языке
            return string.Empty;
        }
    }
}