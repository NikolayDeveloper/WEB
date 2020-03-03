using System;
using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	[TemplateFieldName(TemplateFieldName.CurrentYear)]
	internal class CurrentYear : TemplateFieldValueBase
	{
		public CurrentYear(IExecutor executor) : base(executor)
		{
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };//Здесь не используется
		}

		protected override dynamic GetInternal(Dictionary<string, object> parameters)
		{
			return DateTime.Now.ToString("yyyy");
		}
	}
}