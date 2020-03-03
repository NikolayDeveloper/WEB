using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.DocumentUserInput
{
    public class GetDocumentUserInputByDocumentIdQuery : BaseQuery
    {
        public async Task<Domain.Entities.Document.DocumentUserInput> ExecuteAsync(int documentId)
        {
            var repository = Uow.GetRepository<Domain.Entities.Document.DocumentUserInput>();

            var result = await repository.AsQueryable().FirstOrDefaultAsync(r => r.DocumentId == documentId);

            return result;
        }
    }
}
