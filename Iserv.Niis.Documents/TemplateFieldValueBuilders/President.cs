using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Administration;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
	/// <summary>
	/// Президент(председатель)
	/// </summary>
	[TemplateFieldName(TemplateFieldName.President)]
	internal class President : TemplateFieldValueBase
	{
		public President(IExecutor executor) : base(executor)
		{
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" } ;
		}

		protected override dynamic GetInternal(Dictionary<string, object> parameters)
		{
		    var presidents = Executor.GetQuery<GetUsersByPositionIdQuery>().Process(q => q.Execute(DicPosition.PresidentId));
		    var activePresident = presidents.FirstOrDefault(p => p.IsArchive == false);

		    return activePresident?.NameRu ?? string.Empty;
		}
	}
}