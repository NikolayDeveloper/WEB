using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Info.RequestInfos
{
    public class CreateRequestInfoCommand : BaseCommand
    {
        public async Task ExecuteAsync(RequestInfo requestInfo)
        {
            var requestInfoRepo = Uow.GetRepository<RequestInfo>();
            await requestInfoRepo.CreateAsync(requestInfo);
            await Uow.SaveChangesAsync();
        }
    }
}
