using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request
{
    public class AddIcgsRequestRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<ICGSRequest> icgs)
        {
            var icgsRequestRepo = Uow.GetRepository<ICGSRequest>();
            await icgsRequestRepo.CreateRangeAsync(icgs);
            await Uow.SaveChangesAsync();
        }
    }
}
