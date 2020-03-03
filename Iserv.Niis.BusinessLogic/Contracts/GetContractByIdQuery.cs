using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GetContractByIdQuery : BaseQuery
    {
        public async Task<Contract> ExecuteAsync(int contractId)
        {
            var contractRepository = Uow.GetRepository<Contract>();

            var contract = await contractRepository
                .AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.Type)
                .Include(r => r.ReceiveType)
                .Include(r => r.ContractCustomers).ThenInclude(c => c.CustomerRole)
                .Include(r => r.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                .Include(r => r.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                .Include(r => r.Workflows).ThenInclude(r => r.Route)
                .Include(r => r.RequestsForProtectionDoc).ThenInclude(r => r.Request).ThenInclude(r => r.ProtectionDocType)
                .Include(r => r.RequestsForProtectionDoc).ThenInclude(r => r.Request).ThenInclude(r => r.ICGSRequests).ThenInclude(r => r.Icgs)
                .Include(r => r.RequestsForProtectionDoc).ThenInclude(r => r.ContractRequestICGSRequests).ThenInclude(r => r.ICGSRequest).ThenInclude(r => r.Icgs)
                .Include(r => r.ProtectionDocs).ThenInclude(pd => pd.ProtectionDoc).ThenInclude(pd => pd.Type)
                .Include(r => r.Addressee)
                .Include(r => r.Department)
                .Include(r => r.Division)
                .Include(r => r.MainAttachment)
                .Include(r => r.Documents).ThenInclude(d => d.Document).ThenInclude(t => t.Type)
                .FirstOrDefaultAsync(contr => contr.Id == contractId);

            return contract;
        }
    }
}
