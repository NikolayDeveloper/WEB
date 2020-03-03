using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icis.Request
{
    public class AddIcisRequestRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<int> icisIds)
        {
            var requestRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var request = await requestRepository.GetByIdAsync(requestId);
            foreach (var id in icisIds)
            {
                request.ICISRequests.Add(new ICISRequest{RequestId = requestId, IcisId = id});
            }
            await Uow.SaveChangesAsync();
        }
    }
}
