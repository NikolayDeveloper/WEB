using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using NetCoreCQRS.Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetDoumentUserInputByDocumentIdQuery : BaseQuery
    {
        public DocumentUserInput ExecuteAsync(int documetId)
        {
            var repository = Uow.GetRepository<DocumentUserInput>();

            return repository.AsQueryable().SingleOrDefault(r => r.DocumentId == documetId);
        }
    }
}
