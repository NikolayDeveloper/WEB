using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.Request
{
    public class UpdateIcgsRequestRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<ICGSRequest> newIcgsRequests)
        {
            var repo = Uow.GetRepository<ICGSRequest>();
            var ids = newIcgsRequests.Select(ni => ni.Id).ToList();

            if (!ids.Any()) return;

            var icgsRequests = repo.AsQueryable()
                .Where(i => ids.Contains(i.Id)).ToList();

            if (!newIcgsRequests.Any()) return;

            foreach (var icgsRequest in icgsRequests)
            {
                var newIcgs = newIcgsRequests.FirstOrDefault(i => i.Id == icgsRequest.Id);
                if (newIcgs == null)
                {
                    continue;
                }

                icgsRequest.ClaimedDescription = newIcgs.ClaimedDescription;
                icgsRequest.ClaimedDescriptionEn = newIcgs.ClaimedDescriptionEn;
                icgsRequest.Description = newIcgs.Description;
                icgsRequest.DescriptionKz = newIcgs.DescriptionKz;
                icgsRequest.NegativeDescription = newIcgs.NegativeDescription;
                icgsRequest.IcgsId = newIcgs.IcgsId;
                icgsRequest.RequestId = newIcgs.RequestId;
                icgsRequest.IsRefused = newIcgs.IsRefused;
                icgsRequest.IsPartialRefused = newIcgs.IsPartialRefused;
                icgsRequest.ReasonForPartialRefused = newIcgs.ReasonForPartialRefused;

                repo.Update(icgsRequest);
            }

            await Uow.SaveChangesAsync();
        }
    }
}
