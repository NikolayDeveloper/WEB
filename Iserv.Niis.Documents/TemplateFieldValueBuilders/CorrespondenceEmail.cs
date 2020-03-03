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
    /// e-mail патентный поверенный
    /// </summary>
    [TemplateFieldName(TemplateFieldName.CorrespondenceEmail)]
    internal class CorrespondenceEmail : TemplateFieldValueBase
    {
        public CorrespondenceEmail(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>()
                .Process(q => q.Execute((int)parameters["RequestId"]));
            var addresses = request.RequestCustomers
                .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.Correspondence)
                .Select(x => x.Customer.Email);
            return string.Join(Environment.NewLine, addresses);
        }
    }
}