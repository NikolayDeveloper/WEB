using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request
{
    public class UpdateRequestIcisCommand : BaseCommand
    {
        public void Execute(ICISRequest icisRequest)
        {
            var repo = Uow.GetRepository<ICISRequest>();
            repo.Update(icisRequest);
            Uow.SaveChanges();
        }
    }
}
