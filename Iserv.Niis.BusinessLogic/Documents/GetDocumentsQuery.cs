using System.Linq;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetDocumentsQuery : BaseQuery
    {
        public IQueryable<Document> Execute()
        {
            var repo = Uow.GetRepository<Document>();
            return repo.AsQueryable()
                .Include(r => r.Division)
                .Include(r => r.Department)
                .Include(r => r.Type)
                .Include(r => r.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.Addressee).ThenInclude(c => c.Country)
                .Include(r => r.Status);
        }
    }
}