using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS.Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetContractDocumentByDocumentIdQuery : BaseQuery
    {
        public ContractDocument Execute(int documentId)
        {
            var repository = Uow.GetRepository<ContractDocument>();
            return repository.AsQueryable().Include(cd => cd.Contract)
                .ThenInclude(c => c.CurrentWorkflow)
                .ThenInclude(cw => cw.CurrentStage).ThenInclude(cs => cs.OnlineRequisitionStatus)
                .SingleOrDefault(c => c.DocumentId == documentId);
        }
    }
}
