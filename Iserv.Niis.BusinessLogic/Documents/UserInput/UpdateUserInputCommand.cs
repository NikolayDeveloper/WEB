using System.Threading.Tasks;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Material;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Newtonsoft.Json;

namespace Iserv.Niis.BusinessLogic.Documents.UserInput
{
    public class UpdateUserInputCommand: BaseCommand
    {
        public async Task ExecuteAsync(int documentId, UserInputDto userInputDto)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.DocumentUserInput>();
            var oldUserInput = await repo.AsQueryable()
                .FirstOrDefaultAsync(u => u.DocumentId == documentId);
            if (oldUserInput == null)
            {
                throw new DataNotFoundException(nameof(UserInputDto), DataNotFoundException.OperationType.Read, documentId);
            }
            oldUserInput.UserInput = JsonConvert.SerializeObject(userInputDto);
            repo.Update(oldUserInput);

            await Uow.SaveChangesAsync();
        }
    }
}
