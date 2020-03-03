using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request
{
    public class DeleteRangeICISRequestCommand : BaseCommand
    {
        public async Task ExecuteAsync(List<ICISRequest> icisRequests)
        {
            var icisRequestRepo = Uow.GetRepository<ICISRequest>();
            icisRequestRepo.DeleteRange(icisRequests);
            await Uow.SaveChangesAsync();
        }
    }
}