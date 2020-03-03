using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Attributes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Documents.Enums;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    [TemplateFieldName(TemplateFieldName.PatentOwnerEn)]
    public class PatentOwnerEn : TemplateFieldValueBase
    {
        public PatentOwnerEn(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId", "OwnerType" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            Owner.Type ownerType = (Owner.Type)(int)parameters["OwnerType"];
            int ownerId = (int)parameters["RequestId"];

            IEnumerable<string> patentOwnerNames = new string[] { };

            switch (ownerType)
            {
                case Owner.Type.Request:
                    Request request = Executor
                        .GetQuery<GetRequestByIdQuery>()
                        .Process(query => query.Execute(ownerId));

                    if (request != null)
                    {
                        patentOwnerNames = request.RequestCustomers
                            .Where(IsRequestCustomerPatentOwner())
                            .Select(GetRequestPatentOwnerName());
                    }

                    break;

                case Owner.Type.ProtectionDoc:

                    ProtectionDoc protectionDoc = Executor
                        .GetQuery<GetProtectionDocByIdQuery>()
                        .Process(query => query.Execute(ownerId));

                    if (protectionDoc != null)
                    {
                        patentOwnerNames = protectionDoc.ProtectionDocCustomers
                            .Where(IsProtectionCustomerPatentOwner())
                            .Select(GetProtectionDocPatentOwnerName());
                    }

                    break;
            }

            return string.Join(", ", patentOwnerNames);
        }

        private static Func<RequestCustomer, bool> IsRequestCustomerPatentOwner()
        {
            return customer => customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner;
        }

        private static Func<RequestCustomer, string> GetRequestPatentOwnerName()
        {
            return customer => $"{customer.Customer.NameEn} ({customer.Customer.Country?.Code})";
        }


        private static Func<ProtectionDocCustomer, bool> IsProtectionCustomerPatentOwner()
        {
            return customer => customer.CustomerRole.Code == DicCustomerRoleCodes.PatentOwner;
        }

        private static Func<ProtectionDocCustomer, string> GetProtectionDocPatentOwnerName()
        {
            return customer => $"{customer.Customer.NameEn} ({customer.Customer.Country?.Code})";
        }
    }
}
