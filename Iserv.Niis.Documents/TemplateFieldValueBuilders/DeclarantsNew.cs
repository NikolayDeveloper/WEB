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
	/// Заявители
	/// </summary>
	[TemplateFieldName(TemplateFieldName.DeclarantsNew)]
	internal class DeclarantsNew : TemplateFieldValueBase
	{
		public DeclarantsNew(IExecutor executor) : base(executor)
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
            var customerNames = request.RequestCustomers
				.Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.Declarant)
				.Select(x => x.Customer.NameRu ?? string.Empty)
				.ToArray();
                
			return string.Join(", ", customerNames);
            
            // TODO реализовать Измененное наименование заявителя 
            return string.Empty;
		}
    }
}