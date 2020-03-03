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
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Documents.TemplateFieldValueBuilders
{
    /// <summary>
    /// Представители заявителя
    /// </summary>
    [TemplateFieldName(TemplateFieldName.Attorneys)]
    public class Attorneys: TemplateFieldValueBase
    {
        public Attorneys(IExecutor executor) : base(executor)
        {
        }

        protected override IEnumerable<string> RequiredParameters()
        {
            return new[] { "RequestId" };
        }

        protected override dynamic GetInternal(Dictionary<string, object> parameters)
        {
            List<string> customerNames = null;
            var ownerType = (Owner.Type)(int)parameters["OwnerType"];
            var roles = new[]
                {DicCustomerRoleCodes.Declarant, DicCustomerRoleCodes.Correspondence, DicCustomerRoleCodes.Confidant};
            switch (ownerType)
            {
                case Owner.Type.Request:
                    var request = Executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.Execute((int)parameters["RequestId"]));
                    customerNames = request
                        .RequestCustomers
                        .Where(x => roles.Contains(x.CustomerRole.Code))
                        .Select(x => x.Customer.NameRu ?? string.Empty)
                        .ToList();
                    break;
                case Owner.Type.Contract:
                    var contract = Executor.GetQuery<GetContractByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
                    customerNames = contract
                        .ContractCustomers
                        .Where(x => roles.Contains(x.CustomerRole.Code))
                        .Select(x => x.Customer.NameRu ?? string.Empty)
                        .ToList();
                    break;
                default:
                    var protectionDoc = Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.Execute((int)parameters["RequestId"]));
                    customerNames = protectionDoc
                        .ProtectionDocCustomers
                        .Where(x => roles.Contains(x.CustomerRole.Code))
                        .Select(x => x.Customer.NameRu ?? string.Empty)
                        .ToList();
                    break;
            }
            return string.Join(", ", customerNames);
        }
    }
}
