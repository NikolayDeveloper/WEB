using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	//MOTIV_OTKAZA
	[TemplateFieldName(TemplateFieldName.DisclaimerReason)]
	internal class DisclaimerReason : TemplateFieldValueBase
	{
		public DisclaimerReason(IExecutor executor) : base(executor)
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
		    var icgsCodes = request.ICGSRequests
		        .Where(i => i.IsNegative == true)
		        .Select(i => i.Icgs.Code);
			return string.Join(Environment.NewLine, icgsCodes);
		}
	}
}
