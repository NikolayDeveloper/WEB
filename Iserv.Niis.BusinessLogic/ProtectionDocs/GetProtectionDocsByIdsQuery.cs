using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocsByIdsQuery : BaseQuery
    {
        public IEnumerable<ProtectionDoc> Execute(int[] Ids)
        {
            var repository = Uow.GetRepository<ProtectionDoc>();

			var protectionDoc = repository
				.AsQueryable()
				.Include(pd => pd.Type)
				.Include(pd => pd.SubType)
				.Include(pd => pd.Workflows).ThenInclude(w => w.FromStage)
				.Include(pd => pd.Workflows).ThenInclude(w => w.CurrentStage)
				.Include(pd => pd.Workflows).ThenInclude(w => w.FromUser)
				.Include(pd => pd.Workflows).ThenInclude(w => w.CurrentUser)
				.Include(pd => pd.Workflows).ThenInclude(w => w.Route)
				.Include(pd => pd.Documents).ThenInclude(d => d.Document).ThenInclude(d => d.Type)
				.Include(pd => pd.ProtectionDocInfo)
				.Include(pd => pd.SelectionAchieveType)
				.Include(r => r.ProtectionDocInfo).ThenInclude(i => i.BreedCountry)
				.Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
				.Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
				.Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser).ThenInclude(u => u.Department).ThenInclude(div => div.Division)
				.Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.CustomerRole)
				.Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
				.Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
				.Include(r => r.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
				.Include(r => r.IcisProtectionDocs)
				.Include(r => r.IpcProtectionDocs).ThenInclude(ipc => ipc.Ipc)
				.Include(r => r.ColorTzs)
				.Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
				.Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
				.Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
				.Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
				.Include(r => r.EarlyRegs).ThenInclude(c => c.EarlyRegType)
				.Include(r => r.Icfems).ThenInclude(i => i.DicIcfem)
				.Include(r => r.ProtectionDocConventionInfos)
				.Include(r => r.ConventionType)
				.Include(pd => pd.Bulletins).ThenInclude(pb => pb.Bulletin)
				.Include(pd => pd.Addressee).ThenInclude(a => a.Type)
				.Include(pd => pd.Addressee).ThenInclude(a => a.Country)
				.Where(pd => Ids.Contains(pd.Id));
				


			return protectionDoc;




		}
    }
}
