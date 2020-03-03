using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.SignedUsers)]
    internal class SignedUsers : TemplateFieldValueBase
    {
        public SignedUsers(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var userSignatures = Executor.GetQuery<GetSignaturesByDocumentId>()
                .Process(q => q.Execute((int) parameters["DocumentId"]));
            var result = userSignatures
                .Select(us =>
                    $"{us.DateCreate:yyyy-MM-dd hh:mm:ss} - {us.User?.NameRu ?? string.Empty}  ({us.User?.Department?.NameRu ?? string.Empty})");

            return string.Join(Environment.NewLine, result);
        }
    }
}