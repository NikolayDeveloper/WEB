using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents.ManyToManyRelations
{
    public class AddContractDocumentsCommand: BaseCommand
    {
        public async Task ExecuteAsync(List<ContractDocument> contractDocuments)
        {
            var contractDocumentsRepo = Uow.GetRepository<ContractDocument>();
            await contractDocumentsRepo.CreateRangeAsync(contractDocuments);

            await Uow.SaveChangesAsync();
        }
    }
}
