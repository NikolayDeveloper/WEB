using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	//адрес
	[TemplateFieldName(TemplateFieldName.ProtectionDocNumber)]
	internal class ProtectionDocNumber : TemplateFieldValueBase
	{
		public ProtectionDocNumber(IExecutor executor) : base(executor)
		{
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };
		}

		protected override dynamic GetInternal(Dictionary<string, object> parameters)
		{
			return string.Empty;//ToDo: после разговора с аналитиками приняли решение пока завести пусто
		}
	}
}