using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class DeleteExpertSearchSimilarRangeCommand : BaseCommand
    {
        private readonly IExecutor _executor;

        public DeleteExpertSearchSimilarRangeCommand(IExecutor executor)
        {
            _executor = executor;
        }

        public void Execute(List<ExpertSearchSimilar> similars)
        {
            var repo = Uow.GetRepository<ExpertSearchSimilar>();

            repo.DeleteRange(similars);
            Uow.SaveChanges();
        }
    }
}