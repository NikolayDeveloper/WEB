using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Contracts
{
    public class GetContractByIdQuery : BaseQuery
    {
        public Contract Execute(int contractId)
        {
            var contractRepository = Uow.GetRepository<Contract>();

            var contract = contractRepository
                .AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                .Include(r => r.ProtectionDocType)
                .Include(r => r.Type)
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
                .Include(r => r.Addressee).ThenInclude(d => d.ContactInfos).ThenInclude(d => d.Type)
                .Include(r => r.RequestsForProtectionDoc).ThenInclude(r => r.Request)
                .ThenInclude(r => r.ProtectionDocType)
                .Include(r => r.RequestsForProtectionDoc).ThenInclude(r => r.ContractRequestICGSRequests)
                .ThenInclude(r => r.ICGSRequest).ThenInclude(r => r.Icgs)
                .Include(r => r.ProtectionDocs).ThenInclude(r => r.ProtectionDoc).ThenInclude(r => r.Request)
                .Include(r => r.Addressee)
                .SingleOrDefault(contr => contr.Id == contractId);

            return contract;
        }
    }
}