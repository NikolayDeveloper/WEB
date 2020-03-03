using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Requests
{
    public class GetRequestsByIds: BaseQuery
    {
        public List<Request> Execute(List<int> requestIds)
        {
            var requestRepository = Uow.GetRepository<Request>();

            var requests = requestRepository.AsQueryable()
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
                .Include(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Type)
                .Where(r => requestIds.Contains(r.Id));

            return requests.ToList();
        }
    }
}
