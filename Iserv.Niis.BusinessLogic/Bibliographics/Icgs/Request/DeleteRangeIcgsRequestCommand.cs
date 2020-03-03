using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request
{
    public class DeleteRangeIcgsRequestCommand: BaseCommand
    {
        public async Task ExecuteAsync(List<ICGSRequest> icgsRequests)
        {
            var icgsRequestRepo = Uow.GetRepository<ICGSRequest>();
            icgsRequestRepo.DeleteRange(icgsRequests);
            await Uow.SaveChangesAsync();
        }
    }
}
