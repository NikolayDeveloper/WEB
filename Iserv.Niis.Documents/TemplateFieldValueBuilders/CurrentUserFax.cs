using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Administration;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	//Факс
	[TemplateFieldName(TemplateFieldName.CurrentUserFax)]
	internal class CurrentUserFax : TemplateFieldValueBase
	{
		public CurrentUserFax(IExecutor executor) : base(executor)
		{
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "UserId" };
		}

		protected override dynamic GetInternal(Dictionary<string, object> parameters)
		{
		    var user = Executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute((int)parameters["UserId"]));

		    return user?.Customer?.PhoneFax ?? string.Empty;
        }
	}
}