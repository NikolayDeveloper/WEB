using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
	/// Адрес заявителя
	/// </summary>
	[TemplateFieldName(TemplateFieldName.DeclarantsAddress)]
    internal class DeclarantsAddress : TemplateFieldValueBase
    {
        public DeclarantsAddress(IExecutor executor) : base(executor)
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
            var customerAddress = request.RequestCustomers
                .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.Declarant)
                .Select(x => x.Customer.Address)
                .ToArray();

            return string.Join(Environment.NewLine, customerAddress);
        }
    }
}
