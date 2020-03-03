using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Патентообладатель
    /// </summary>
    [TemplateFieldName(TemplateFieldName.PatentOwnerCountryCodesRu)]
    internal class PatentOwnerCountryCodesRu : TemplateFieldValueBase
    {
        public PatentOwnerCountryCodesRu(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute((int)parameters["RequestId"]));

            return request.RequestCustomers
                .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.PatentOwner)
                .Select(x => $"{x.Customer.NameRu} {x.Customer.Country.Code}")
                .Join();
        }
    }
}
