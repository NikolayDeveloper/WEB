using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class ClearExpertSearchSimilarsCommand : BaseCommand
    {
        public void Execute(int requestId)
        {
            var repo = Uow.GetRepository<ExpertSearchSimilar>();
            var similars = repo.AsQueryable().Where(s => s.RequestId == requestId);
            repo.DeleteRange(similars);
            Uow.SaveChanges();
        }
    }
}