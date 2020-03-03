using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class UpdateExpertSearchSimilarsCommand : BaseCommand
    {
        public void Execute(List<ExpertSearchSimilar> similars)
        {
            var repo = Uow.GetRepository<ExpertSearchSimilar>();
            repo.UpdateRange(similars);
            Uow.SaveChanges();
        }
    }
}