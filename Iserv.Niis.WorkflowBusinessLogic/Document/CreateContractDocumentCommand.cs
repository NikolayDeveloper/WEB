using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class CreateContractDocumentCommand: BaseCommand
    {
        public void Execute(ContractDocument contractDocument)
        {
            var repo = Uow.GetRepository<ContractDocument>();

            repo.Create(contractDocument);
            Uow.SaveChanges();
        }
    }
}
