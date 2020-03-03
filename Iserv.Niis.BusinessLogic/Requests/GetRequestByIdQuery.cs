using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Requests
{
    /// <summary>
    /// Запрос для получения заявки по его идентификатору.
    /// </summary>
    public class GetRequestByIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns>Заявка.</returns>
        public async Task<Request> ExecuteAsync(int requestId)
        {
            var repository = Uow.GetRepository<Request>();

            var result = repository.AsQueryable()
                .Include(r => r.RequestInfo).ThenInclude(i => i.BreedCountry)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser).ThenInclude(u => u.Department).ThenInclude(div => div.Division)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromUser)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.CustomerRole)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.BeneficiaryType)
                .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                .Include(r => r.Addressee).ThenInclude(d => d.ContactInfos)
                .Include(r => r.ICGSRequests).ThenInclude(i => i.Icgs)
                .Include(r => r.ICISRequests)
                .Include(r => r.IPCRequests).ThenInclude(i => i.Ipc)
                .Include(r => r.ColorTzs)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                .Include(r => r.EarlyRegs).ThenInclude(c => c.EarlyRegType)
                .Include(r => r.Icfems).ThenInclude(i => i.DicIcfem)
                .Include(r => r.RequestType)
                .Include(r => r.RequestConventionInfos)
                .Include(r => r.Department)
                .Include(r => r.MainAttachment)
                .Include(r => r.ConventionType)
                .Include(r => r.ReceiveType)
                .Include(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Type)
                .Include(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Addressee)
                .Include(r => r.SelectionAchieveType)
                .Include(r => r.SpeciesTradeMark)
                .Include(r => r.TypeTrademark)
                .Include(r => r.ParentRequests)
                .Include(r => r.ChildsRequests)
                .Include(r => r.MediaFiles)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            return await result;
        }
    }
}