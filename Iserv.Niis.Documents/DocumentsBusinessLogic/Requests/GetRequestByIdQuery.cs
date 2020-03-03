using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Requests
{
    /// <summary>
    /// Возвращает заявку по её идентификатору.
    /// </summary>
    public class GetRequestByIdQuery: BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns>Заявка.</returns>
        public Request Execute(int requestId)
        {
            var requestRepository = Uow.GetRepository<Request>();

            var request = requestRepository.AsQueryable()
                .Include(r => r.RequestInfo).ThenInclude(i => i.BreedCountry)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.CustomerRole)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                .Include(r => r.Addressee).ThenInclude(d => d.ContactInfos).ThenInclude(d => d.Type)
                .Include(r => r.ICGSRequests).ThenInclude(ir => ir.Icgs)
                .Include(r => r.ICISRequests).ThenInclude(ic => ic.Icis)
                .Include(r => r.IPCRequests).ThenInclude(ipc => ipc.Ipc)
                .Include(r => r.ColorTzs).ThenInclude(c => c.ColorTz)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                .Include(r => r.EarlyRegs).ThenInclude(c => c.EarlyRegType)
                .Include(r => r.Icfems).ThenInclude(ir => ir.DicIcfem)
                .Include(r => r.RequestType)
                .Include(r => r.RequestConventionInfos)
                .Include(r => r.Department)
                .SingleOrDefault(r => r.Id == requestId);

            return request;
        }
    }
}
