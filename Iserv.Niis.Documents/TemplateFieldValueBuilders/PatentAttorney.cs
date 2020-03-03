using System;
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
    [TemplateFieldName(TemplateFieldName.PatentAttorney)]
    internal class PatentAttorney : TemplateFieldValueBase
    {
        public PatentAttorney(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var patentAttorneys = request.RequestCustomers
                        .Where(r => r.CustomerRole.Code == DicCustomerRole.Codes.PatentAttorney)
                        .Select(x => x.Customer.NameRu);

                    return string.Join(Environment.NewLine, patentAttorneys);
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var pdPatentAttorneys = protectionDoc.ProtectionDocCustomers
                        .Where(r => r.CustomerRole.Code == DicCustomerRole.Codes.PatentAttorney)
                        .Select(x => x.Customer.NameRu);

                    return string.Join(Environment.NewLine, pdPatentAttorneys);
                default:
                    return string.Empty;
            }


            
        }
    }
}