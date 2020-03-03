using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Заявители.
    /// </summary>
    [TemplateFieldName(TemplateFieldName.DeclarantsEn)]
    internal class DeclarantsEn : TemplateFieldValueBase
    {
        public DeclarantsEn(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            IEnumerable<string> customerNames;

            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            var ownerId = (int)parameters["RequestId"];

            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor
                        .GetQuery<GetRequestByIdQuery>()
                        .Process(query => query.Execute(ownerId));

                    customerNames = request
                        .RequestCustomers
                        .Where(IsRequestCustomerDeclarant())
                        .Select(customer => $"{customer.Customer.NameEn} ({customer.Customer.Country?.Code})");
                    break;

                case Owner.Type.Contract:
                    var contract = Executor
                        .GetQuery<GetContractByIdQuery>()
                        .Process(query => query.Execute(ownerId));

                    customerNames = contract
                        .ContractCustomers
                        .Where(IsContractCustomerDeclarant())
                        .Select(customer => $"{customer.Customer.NameEn} ({customer.Customer.Country?.Code})");
                    break;

                default:
                    var protectionDoc = Executor
                        .GetQuery<GetProtectionDocByIdQuery>()
                        .Process(query => query.Execute(ownerId));

                    customerNames = protectionDoc
                        .ProtectionDocCustomers
                        .Where(IsProtectionDocCustomerDeclarant())
                        .Select(customer => $"{customer.Customer.NameEn} ({customer.Customer.Country?.Code})");
                    break;
            }

            return string.Join(", ", customerNames);
        }

        private static Func<RequestCustomer, bool> IsRequestCustomerDeclarant()
        {
            return customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant;
        }

        private static Func<ProtectionDocCustomer, bool> IsProtectionDocCustomerDeclarant()
        {
            return customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant;
        }

        private static Func<ContractCustomer, bool> IsContractCustomerDeclarant()
        {
            return customer => customer.CustomerRole.Code == DicCustomerRoleCodes.Declarant;
        }
    }
}
