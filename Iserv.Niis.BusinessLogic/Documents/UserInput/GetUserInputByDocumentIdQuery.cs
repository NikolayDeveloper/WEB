using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Documents.UserInput
{
    public class GetUserInputByDocumentIdQuery: BaseQuery
    {
        public async Task<Domain.Entities.Document.DocumentUserInput> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<Domain.Entities.Document.DocumentUserInput>();
            var userInput = await repo.AsQueryable()
                .FirstOrDefaultAsync(u => u.DocumentId == documentId);

            return userInput;
        }
    }
}
