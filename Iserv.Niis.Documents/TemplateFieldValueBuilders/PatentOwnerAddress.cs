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
	[TemplateFieldName(TemplateFieldName.PatentOwnerAddress)]
	internal class PatentOwnerAddress : TemplateFieldValueBase
	{
		public PatentOwnerAddress(IExecutor executor) : base(executor)
		{
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };
		}

		protected override dynamic GetInternal(Dictionary<string, object> parameters)
		{
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            string patentOwnerAddresses;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var customerNames = request
                        .RequestCustomers
                        .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.PatentOwner)
                        .Select(s => $"{s.Customer.Address} {s.Customer.Country?.ToString() ?? string.Empty}")
                        .ToArray();
                    patentOwnerAddresses = string.Join(", ", customerNames);
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    var protectionDocCustomers = protectionDoc
                        .ProtectionDocCustomers
                        .Where(x => x.CustomerRole.Code == DicCustomerRole.Codes.PatentOwner)
                        .Select(s => $"{s.Customer.Address} {s.Customer.Country?.ToString() ?? string.Empty}")
                        .ToArray();
                    patentOwnerAddresses = string.Join(", ", protectionDocCustomers);
                    break;
                default:
                    patentOwnerAddresses = string.Empty;
                    break;
            }


            return patentOwnerAddresses;
		}
	}
}