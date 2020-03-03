using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	/// <summary>
	/// Заявители и адрес
	/// </summary>
	[TemplateFieldName(TemplateFieldName.DeclarantsAndAddress)]
	internal class DeclarantsAndAddress : TemplateFieldValueBase
	{
		public DeclarantsAndAddress(IExecutor executor) : base(executor)
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
            var customerNamesAndAddress = request.RequestCustomers
                .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.Declarant)
                .Select(x => x.Customer.NameRu + " (" + $"{x.Customer.Country.Code}" + ")" + Environment.NewLine + x.Customer.Address) 
                .ToArray();

            return string.Join(Environment.NewLine, customerNamesAndAddress);
        }
    }
}
