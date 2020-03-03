using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    /// <summary>
    /// Класс, представляющий запрос на получение заявки по его идентификатору.
    /// </summary>
    public class GetRequestByIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns></returns>
        public Request Execute(int requestId)
        {
            var repository = Uow.GetRepository<Request>();

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.FromStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser).ThenInclude(u => u.Department).ThenInclude(div => div.Division)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.Route)
                .Include(r => r.PaymentInvoices)
                .Include(r => r.RequestCustomers)
                .Include(r => r.RequestInfo).ThenInclude(i => i.BreedCountry)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.CustomerRole)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.BeneficiaryType)
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
                .Include(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Workflows)
                .Include(r => r.ConventionType)
                .Include(r => r.SelectionAchieveType)
                .Include(r => r.SpeciesTradeMark)
                .Include(r => r.ReceiveType)
                .Include(r => r.BeneficiaryType)
                .FirstOrDefault(r => r.Id == requestId);
        }
    }
}
