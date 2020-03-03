using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Documents.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	//Дата прописью
	//Изначально планировалось полностью преобразовывать дату в пропись, но временно отказались, так как НИИС предерживается формата "01 января 2015"
	[TemplateFieldName(TemplateFieldName.DateInWords)]
	internal class DateInWords : TemplateFieldValueBase
	{
		public DateInWords(IExecutor executor) : base(executor)
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

            return request?.DateCreate.ToTemplateDateFormatWithWordMonth() ?? string.Empty;
		}
	}
}