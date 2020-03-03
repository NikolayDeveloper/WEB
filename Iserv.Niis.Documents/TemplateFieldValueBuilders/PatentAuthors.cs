using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.PatentAuthors)]
    public class PatentAuthors : TemplateFieldValueBase

    {
        public PatentAuthors(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "OwnerType" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            var ownerId = (int)parameters["RequestId"];

            string[] authors = { };

            switch (ownerType)
            {
                case Owner.Type.Request:

                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute(ownerId));

                    if (request is null)
                    {
                        return string.Empty;
                    }

                    authors = request.RequestCustomers
                        .Where(customer => customer.RequestId == ownerId &&
                                           customer.CustomerRole.Code == DicCustomerRole.Codes.Author)
                        .Select(customer => customer.Customer?.NameRu ?? string.Empty)
                        .ToArray();

                    break;

                case Owner.Type.ProtectionDoc:

                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute(ownerId));

                    if (protectionDoc is null)
                    {
                        return string.Empty;
                    }

                    authors = protectionDoc.ProtectionDocCustomers
                        .Where(customer => customer.ProtectionDocId == ownerId &&
                                           customer.CustomerRole.Code == DicCustomerRole.Codes.Author)
                        .Select(customer => customer.Customer?.NameRu ?? string.Empty)
                        .ToArray();

                    break; ;
            }

            if (authors.Length > 1)
            {
                return $"{string.Join(", ", authors, 0, authors.Length - 1)} и {authors.LastOrDefault()}";
            }

            return authors.FirstOrDefault() ?? string.Empty;
        }
    }
}
