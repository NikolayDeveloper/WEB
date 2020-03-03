using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.Request
{
    public class AddRequestConventionInfosRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestid, List<RequestConventionInfo> infos)
        {
            var requestRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var request = await requestRepository.GetByIdAsync(requestid);
            foreach (var info in infos)
            {
                request.RequestConventionInfos.Add(info);
            }
            await Uow.SaveChangesAsync();
        }
    }
}
