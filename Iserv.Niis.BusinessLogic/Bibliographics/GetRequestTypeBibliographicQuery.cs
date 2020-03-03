using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.BibliographicData;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics
{
    public class GetRequestTypeBibliographicQuery: BaseQuery
    {
        private readonly IMapper _mapper;

        public GetRequestTypeBibliographicQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<BibliographicDataDto> ExecuteAsync(int ownerId)
        {
            var requestRepository = Uow.GetRepository<Request>();

            var request = await requestRepository
                .AsQueryable()
                .Include(r => r.RequestInfo).ThenInclude(i => i.BreedCountry)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.CustomerRole)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                .Include(r => r.Addressee)
                .Include(r => r.ICGSRequests).ThenInclude(i => i.Icgs)
                .Include(r => r.ICISRequests)
                .Include(r => r.IPCRequests)
                .Include(r => r.ColorTzs)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                .Include(r => r.Icfems)
                .Include(r => r.RequestType)
                .Include(r => r.RequestConventionInfos)
                .Include(r => r.Department)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Type)
                .Include(r => r.SelectionAchieveType)
                .FirstOrDefaultAsync(req => req.Id == ownerId);

            return _mapper.Map<Request, BibliographicDataDto>(request);
        }
    }
}
