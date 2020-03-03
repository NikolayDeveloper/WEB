using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Documents;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    //[TemplateFieldName(TemplateFieldName.CustomerPhone)]
    //internal class CustomerPhone : TemplateFieldValueBase
    //{
    //}
    [TemplateFieldName(TemplateFieldName.CustomerPhone)]
    internal class CustomerPhone : TemplateFieldValueBase
    {
        public CustomerPhone(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "DocumentId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var documentId = (int)parameters["DocumentId"];
            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
            var customerId = document?.AddresseeId ?? 0;
            if (customerId == 0)
            {
                return string.Empty;
            }
            var customerContactInfo = Executor.GetQuery<GetCustomerContactInfoByIdQuery>().Process(q => q.Execute(customerId));

            var mobilePhoneArray = customerContactInfo.Where(d => d.Type.Code == DicContactInfoType.Codes.MobilePhone);
            return string.Join(", ", mobilePhoneArray.Select(d => d.Info));
        }
    }

}
