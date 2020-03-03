using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.ContractStorona2NameRu)]
    internal class ContractStorona2NameRu : TemplateFieldValueBase
    {
        public ContractStorona2NameRu(IExecutor executor) : base(executor)
        {

        }
        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }
        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
            return contract.ContractCustomers.LastOrDefault(cc => cc.CustomerRole.Code == DicCustomerRoleCodes.SideTwo)?.Customer.NameRu ?? string.Empty;
        }
    }
}
