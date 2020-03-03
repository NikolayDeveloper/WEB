using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations
{
    public class RemoveContractDocumentsCommand:BaseCommand
    {
        public async Task ExecuteAsync(List<ContractDocument> contractDocuments)
        {
            var contractDocumentsRepo = Uow.GetRepository<ContractDocument>();
            var contractDocumentsToDelete = contractDocumentsRepo.AsQueryable()
                .Where(x => contractDocuments.Select(rd => rd.Id).Contains(x.Id));
            contractDocumentsRepo.DeleteRange(contractDocumentsToDelete);

            await Uow.SaveChangesAsync();
        }
    }
}
