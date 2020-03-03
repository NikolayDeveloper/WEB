using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs
{
    public class GetProtectionDocByIdQuery: BaseQuery
    {
        public ProtectionDoc Execute(int protectionDocId)
        {
            var protectionDocRepository = Uow.GetRepository<ProtectionDoc>();
            var protectionDoc = protectionDocRepository
                .AsQueryable()
                .Include(pd => pd.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(pd => pd.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(pd => pd.Type)
                .Include(pd => pd.SubType)
                .Include(pd => pd.Workflows).ThenInclude(w => w.FromStage)
                .Include(pd => pd.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(pd => pd.Workflows).ThenInclude(w => w.FromUser)
                .Include(pd => pd.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(pd => pd.Workflows).ThenInclude(w => w.Route)
                .Include(pd => pd.BeneficiaryType)
                .Include(r => r.Addressee).ThenInclude(d => d.ContactInfos).ThenInclude(d => d.Type)
                .Include(pd => pd.IpcProtectionDocs).ThenInclude(ipd => ipd.Ipc)
                .Include(c => c.ProtectionDocCustomers).ThenInclude(c => c.CustomerRole)
                .Include(c => c.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                .Include(c => c.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                .Include(pd => pd.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
                .Include(pd => pd.Bulletins).ThenInclude(pb => pb.Bulletin)
                .Include(pd => pd.ProtectionDocConventionInfos).ThenInclude(pd => pd.Country)
                .Include(pd => pd.ColorTzs).ThenInclude(c => c.ColorTz)
                .Include(pd => pd.EarlyRegs).ThenInclude(e => e.EarlyRegType)
                .Include(pd => pd.EarlyRegs).ThenInclude(e => e.RegCountry)
                .SingleOrDefault(pd => pd.Id == protectionDocId);

            return protectionDoc;
        }
    }
}
