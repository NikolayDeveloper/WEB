using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.CorrespondenceAddress)]
    internal class CorrespondenceAddress : TemplateFieldValueBase
    {
        public CorrespondenceAddress(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] {"RequestId"};
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            List<string> addresses = null;
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                     addresses = request.RequestCustomers
                        .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.Correspondence)
                        .Select(x => x.Customer.Address)
                        .ToList();
                    break;
                case Owner.Type.Contract:
                    var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
                    addresses = contract.ContractCustomers
                        .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.Correspondence)
                        .Select(x => x.Customer.Address)
                        .ToList();
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
                    addresses = protectionDoc.ProtectionDocCustomers
                        .Where(r => r.CustomerRole.Code == DicCustomerRole.Codes.Correspondence)
                        .Select(x => x.Customer.Address)
                        .ToList();
                    break;
                default:
                    throw new ArgumentNullException(nameof(ownerType));
            }
            return string.Join(Environment.NewLine, addresses);
        }
    }
}