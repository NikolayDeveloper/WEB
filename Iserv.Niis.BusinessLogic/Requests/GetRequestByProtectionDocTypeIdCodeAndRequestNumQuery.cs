using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestByProtectionDocTypeIdCodeAndRequestNumQuery : BaseQuery
    {
        public async Task<Request> ExecuteAsync(int protectionDocTypeId, string requestNum)
        {
            var repository = Uow.GetRepository<Request>();

            var result = repository.AsQueryable()
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
                .FirstOrDefaultAsync(r =>
                    r.ProtectionDocTypeId.Equals(protectionDocTypeId) && r.RequestNum.Equals(requestNum));

            return await result;
        }
    }
}