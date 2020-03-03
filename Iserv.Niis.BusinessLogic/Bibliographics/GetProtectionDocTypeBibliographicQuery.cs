using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Model.Models.BibliographicData;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics
{
    public class GetProtectionDocTypeBibliographicQuery : BaseQuery
    {
        private readonly IMapper _mapper;

        public GetProtectionDocTypeBibliographicQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<BibliographicDataDto> ExecuteAsync(int protectionDocId)
        {
            var requestRepository = Uow.GetRepository<ProtectionDoc>();

            var protectionDoc = await requestRepository
                .AsQueryable()
                .Include(r => r.ProtectionDocInfo).ThenInclude(i => i.BreedCountry)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(r => r.Type)
                .Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.CustomerRole)
                .Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                .Include(r => r.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                .Include(r => r.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
                .Include(r => r.IcisProtectionDocs)
                .Include(r => r.IpcProtectionDocs)
                .Include(r => r.ColorTzs)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                .Include(r => r.Icfems)
                .Include(r => r.SubType)
                .Include(r => r.ProtectionDocConventionInfos)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Type)
                .Include(r => r.SelectionAchieveType)
                .Include(pd => pd.Bulletins).ThenInclude(pb => pb.Bulletin)
                .FirstOrDefaultAsync(doc => doc.Id == protectionDocId);

            return _mapper.Map<ProtectionDoc, BibliographicDataDto>(protectionDoc);
        }
    }
}
