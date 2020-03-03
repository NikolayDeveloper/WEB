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
    /// Телефакс заявителя
    /// </summary>
	[TemplateFieldName(TemplateFieldName.ApplicantPhoneFax)]
	internal class ApplicantPhoneFax : TemplateFieldValueBase
	{
		public ApplicantPhoneFax(IExecutor executor) : base(executor)
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
            var customerFax = request.RequestCustomers
				.Where(r => r.CustomerRole.Code == DicCustomerRole.Codes.Declarant && r.CustomerId.HasValue)
				.Select(s => s.Customer.PhoneFax);

			return string.Join(Environment.NewLine, customerFax);
		}
	}
}