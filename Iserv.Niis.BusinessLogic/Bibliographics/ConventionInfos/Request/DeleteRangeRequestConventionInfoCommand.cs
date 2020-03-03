using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.Request
{
    public class DeleteRangeRequestConventionInfoCommand : BaseCommand
    {
        public async Task ExecuteAsync(List<RequestConventionInfo> requestConventionInfos)
        {
            var requestConventionInfoRepo = Uow.GetRepository<RequestConventionInfo>();
            requestConventionInfoRepo.DeleteRange(requestConventionInfos);
            await Uow.SaveChangesAsync();
        }
    }
}