using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.CustomerName)]
    internal class CustomerName : TemplateFieldValueBase
    {
        public CustomerName(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var parentId = GetDocFirstParentId((int)parameters["RequestId"]);
            var parentRequest = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(parentId));
            var customerId = parentRequest?.AddresseeId ?? 0;
            if (customerId == 0)
            {
                return string.Empty;
            }
            var customer = Executor.GetQuery<GetCustomerByIdQuery>().Process(q => q.Execute(customerId));
            return customer?.NameRu ?? string.Empty;
        }

        private int GetDocFirstParentId(int documentId)
        {
            var relations = Executor.GetQuery<GetRequestRelationsByChildId>().Process(q => q.Execute(documentId));

            var parentId = relations
                .OrderBy(d => d.DateCreate)
                .Select(d => d.ParentId)
                .FirstOrDefault();
            return parentId != 0 ? parentId : documentId;
        }
    }
}