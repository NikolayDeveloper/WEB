using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Newtonsoft.Json;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class CreateUserInputCommand: BaseCommand
    {
        public void Execute(int documentId, UserInputDto userInputDto)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.DocumentUserInput>();
            var input = new Domain.Entities.Document.DocumentUserInput
            {
                DocumentId = documentId,
                UserInput = JsonConvert.SerializeObject(userInputDto)
            };
            repo.Create(input);

            Uow.SaveChanges();
        }
    }
}
