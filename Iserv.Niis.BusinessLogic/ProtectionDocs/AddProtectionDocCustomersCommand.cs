using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Exceptions;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class AddProtectionDocCustomersCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<ProtectionDocCustomer> customers)
        {
            string roleCode;

            var protectionDocRepository = Uow.GetRepository<ProtectionDoc>();
            var protectionDoc = protectionDocRepository.AsQueryable()
                .Include(pd => pd.Type)
                .FirstOrDefault(pd => pd.Id == protectionDocId);

            if (protectionDoc == null)
                throw new DataNotFoundException(nameof(ProtectionDoc), DataNotFoundException.OperationType.Read, protectionDocId);
            
            if (protectionDoc.Type.Code == DicProtectionDocTypeCodes.RequestTypeTrademarkCode ||
                protectionDoc.Type.Code == DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode)
            {
                roleCode = DicCustomerRoleCodes.Owner;
            }
            else
            {
                roleCode = DicCustomerRoleCodes.PatentOwner;
            }

            var roleRepository = Uow.GetRepository<DicCustomerRole>();
            var owner = await roleRepository.AsQueryable()
                .FirstOrDefaultAsync(r => r.Code == roleCode);

            if (owner == null)
            {
                throw new DataNotFoundException(nameof(DicCustomerRole), DataNotFoundException.OperationType.Read, roleCode);
            }

            var protectionDocCustomerRepository = Uow.GetRepository<ProtectionDocCustomer>();
            foreach (var customer in customers)
            {
                var customerRole = await roleRepository.GetByIdAsync(customer.CustomerRoleId);
                customer.ProtectionDocId = protectionDocId;
                customer.CustomerRoleId = customerRole.Code == DicCustomerRoleCodes.Declarant ? owner.Id : customer.CustomerRoleId;
            }
            await protectionDocCustomerRepository.CreateRangeAsync(customers);
            await Uow.SaveChangesAsync();
        }
    }
}
