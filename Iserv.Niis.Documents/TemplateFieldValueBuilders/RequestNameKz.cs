using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	//Наименование на казахском языке
	[TemplateFieldName(TemplateFieldName.RequestNameKz)]
	internal class RequestNameKz : TemplateFieldValueBase
	{
		public RequestNameKz(IExecutor executor) : base(executor)
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

		    return request?.NameKz ?? string.Empty;
        }
	}
}