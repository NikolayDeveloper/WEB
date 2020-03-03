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
	[TemplateFieldName(TemplateFieldName.CorrespondenceContact)]
	internal class CorrespondenceContact : TemplateFieldValueBase
	{
		public CorrespondenceContact(IExecutor executor) : base(executor)
		{
		}

		protected override IEnumerable<string> RequiredParameters()
		{
			return new[] { "RequestId" };
		}

		protected override dynamic GetInternal(Dictionary<string, object> parameters)
		{
		    List<string> customerInfos = null;
          var ownerType = (Owner.Type)(int)parameters["OwnerType"];
		    switch (ownerType)
		    {
		        case Owner.Type.Request:
		            var request = Executor.GetQuery<GetRequestByIdQuery>()
		                .Process(q => q.Execute((int)parameters["RequestId"]));
		            customerInfos = request.RequestCustomers
		                .Where(r => r.CustomerRole.Code == DicCustomerRole.Codes.Correspondence)
		                .Select(se => $"{se.Customer.NameRu}"
                  //$" {se.Customer.NameKz} {se.Customer.NameEn}"
                        )
		                .ToList();
		            break;
                case Owner.Type.Contract:
                    var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
                    customerInfos = contract.ContractCustomers
                        .Where(r => r.CustomerRole.Code == DicCustomerRole.Codes.Correspondence)
                        .Select(se => $"{se.Customer.NameRu} " 
                  //$" {se.Customer.NameKz} {se.Customer.NameEn}"
                        )
                        .ToList();
                    break;
                case Owner.Type.ProtectionDoc:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
                    customerInfos = protectionDoc.ProtectionDocCustomers
                        .Where(r => r.CustomerRole.Code == DicCustomerRole.Codes.Correspondence)
                        .Select(se => $"{se.Customer.NameRu} " 
                  // $" {se.Customer.NameKz} {se.Customer.NameEn}"
                        )
                        .ToList();
                    break;
                default:
                      throw new ArgumentNullException(nameof(ownerType));
            }
           

			return string.Join(Environment.NewLine, customerInfos);
		}
	}
}