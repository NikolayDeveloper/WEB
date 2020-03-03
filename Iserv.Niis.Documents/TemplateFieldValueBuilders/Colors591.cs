using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	struct tests
	{
		public string str;
	}

	class testc
	{
		public string str;
	}
	[TemplateFieldName(TemplateFieldName.Colors)]
	internal class Colors591 : TemplateFieldValueBase
	{
		public Colors591(IExecutor executor) : base(executor)
		{
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };
		}

		protected override dynamic GetInternal(Dictionary<string, object> parameters)
		{
		    

		    var ownerType = (Owner.Type)(int)parameters["OwnerType"];
		    switch (ownerType)
		    {
		        case Owner.Type.Request:
		            var request = Executor.GetQuery<GetRequestByIdQuery>()
		                .Process(q => q.Execute((int)parameters["RequestId"]));
		            var colorTzs = request.ColorTzs;
		            return string.Join(", ", colorTzs.Select(c => c.ColorTz.NameRu));
		        case Owner.Type.ProtectionDoc:
		            var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
		                .Process(q => q.Execute((int)parameters["RequestId"]));
		            var protectionDocColorTzs = protectionDoc.ColorTzs;
		            return string.Join(", ", protectionDocColorTzs.Select(c => c.ColorTz.NameRu));
                default:
		            return string.Empty;
		    }
        }
	}
}