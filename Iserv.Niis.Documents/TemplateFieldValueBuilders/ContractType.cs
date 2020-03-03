using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;


namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.ContractType)]
    internal class ContractType : TemplateFieldValueBase
    {
        public ContractType(IExecutor executor) : base(executor)
        {

        }
        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var contract  = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
            return contract?.Type.NameRu ?? string.Empty;
        }
    }
}
