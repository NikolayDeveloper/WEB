using System.Linq;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocsQuery : BaseQuery
    {
        public IQueryable<ProtectionDoc> Execute()
        {
            var protectionDocs = Uow.GetRepository<ProtectionDoc>()
                .AsQueryable()
                .Include(r => r.SubType)
                .Include(r => r.Type)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.ProtectionDocCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(r => r.ProtectionDocCustomers).ThenInclude(pdc => pdc.CustomerRole);
            
            return protectionDocs;
        }

        public IQueryable<ProtectionDoc> ExecuteByRequestTypeId(int typeId)
        {
            var pDTypeRepo = Uow.GetRepository<DicProtectionDocType>();

            var requestTypeCode = pDTypeRepo.AsQueryable().FirstOrDefault(t => t.Id == typeId).Code;
            var protectionDocType = pDTypeRepo.AsQueryable().LastOrDefault(t => t.Code.StartsWith(requestTypeCode));

            var protectionDocs = Uow.GetRepository<ProtectionDoc>()
                .AsQueryable()
                .Include(r => r.SubType)
                .Include(r => r.Type)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.ProtectionDocCustomers).ThenInclude(rc => rc.Customer).ThenInclude(c => c.Country)
                .Include(r => r.ProtectionDocCustomers).ThenInclude(pdc => pdc.CustomerRole)
                .Where(p => p.TypeId == protectionDocType.Id);

            return protectionDocs;
        }
    }
}