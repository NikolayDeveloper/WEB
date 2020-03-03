using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class AddProtectionDocCustomersCommand : BaseCommand
    {
        public void Execute(int protectionDocId, List<ProtectionDocCustomer> customers)
        {
            var protectionDocRepository = Uow.GetRepository<ProtectionDoc>();
            var protectionDoc = protectionDocRepository.AsQueryable()
                .Include(pd => pd.Type)
                .FirstOrDefault(pd => pd.Id == protectionDocId);

            if (protectionDoc == null)
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, protectionDocId);

            string roleCode;
            if (protectionDoc.Type.Code == DicProtectionDocTypeCodes.RequestTypeTrademarkCode ||
                protectionDoc.Type.Code == DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode ||
                protectionDoc.Type.Code == DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode ||
                protectionDoc.Type.Code == DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode)
            {
                roleCode = DicCustomerRoleCodes.Owner;
            }
            else
            {
                roleCode = DicCustomerRoleCodes.PatentOwner;
            }

            var roleRepository = Uow.GetRepository<DicCustomerRole>();
            var owner = roleRepository.AsQueryable().FirstOrDefault(r => r.Code == roleCode);

            if (owner == null)
            {
                throw new DataNotFoundException(nameof(DicCustomerRole), DataNotFoundException.OperationType.Read, roleCode);
            }

            var protectionDocCustomerRepository = Uow.GetRepository<ProtectionDocCustomer>();

            foreach (var customer in customers)
            {
                var customerRole = roleRepository.GetById(customer.CustomerRoleId);
                customer.ProtectionDocId = protectionDocId;
                if (customerRole.Code == DicCustomerRoleCodes.Declarant)
                {
                    var patentOwner = new ProtectionDocCustomer
                    {
                        CustomerId = customer.CustomerId,
                        ProtectionDocId = protectionDocId,
                        Address = customer.Address,
                        CustomerRoleId = owner.Id,
                        Phone = customer.Phone,
                        PhoneFax = customer.PhoneFax,
                        MobilePhone = customer.MobilePhone,
                        Email = customer.Email
                    };
                    protectionDocCustomerRepository.Create(patentOwner);
                }
            }
            protectionDocCustomerRepository.CreateRange(customers);

            Uow.SaveChanges();
        }
    }
}
