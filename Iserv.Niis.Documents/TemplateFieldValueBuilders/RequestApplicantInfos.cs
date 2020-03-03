using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Models;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Microsoft.EntityFrameworkCore.Internal;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Год продления по счету (2, 3, 4 и т.д.)
    /// </summary>
    [TemplateFieldName(TemplateFieldName.RequestApplicantInfoRecords_Complex)]
    internal class RequestApplicantInfoRecords : TemplateFieldValueBase
    {

        public RequestApplicantInfoRecords(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var requests = Executor.GetQuery<GetRequestsByIds>()
                .Process(q => q.Execute((List<int>)parameters["SelectedRequestIds"]));

            return requests.Select(r => new RequestApplicantInfoRecord
            {
                RequestNum = r.RequestNum,

                DeclarantShortInfo = r.RequestCustomers
                    .Where(rc => rc.CustomerRole.Code == DicCustomerRole.Codes.Declarant)
                    .Select(rc => $"{rc.Customer.NameRu}, ({rc.Customer.Country.Code})").Join()
            });
        }
    }
}