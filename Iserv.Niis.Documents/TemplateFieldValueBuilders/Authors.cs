using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Получает авторов заявки.
    /// </summary>
    [TemplateFieldName(TemplateFieldName.Authors)]
    internal class Authors : TemplateFieldValueBase
    {
        public Authors(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var requestId = (int)parameters["RequestId"];

            var request = Executor
                .GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute(requestId));

            var requestAuthors = request.RequestCustomers
                .Where(customer => customer.RequestId == requestId &&
                                   customer.CustomerRole.Code == DicCustomerRole.Codes.Author)
                .Select(x => $"{x.Customer.NameRu} ({x.Customer.Country.Code})");

            return string.Join(Environment.NewLine, requestAuthors);
        }
    }
}