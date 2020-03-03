using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.Request
{
    public class UpdateRequestConventionInfosRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<RequestConventionInfo> infos)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var requestConventionInfoRepo = Uow.GetRepository<RequestConventionInfo>();
            var request = await protectionDocRepository
                .AsQueryable()
                .Include(r => r.RequestConventionInfos)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null || infos == null || !infos.Any())
            {
                return;
            }

            foreach (var itemConventionInfo in infos)
            {
                var originConventionInfo = requestConventionInfoRepo.GetById(itemConventionInfo.Id);

                originConventionInfo.CountryId = itemConventionInfo.CountryId;
                originConventionInfo.EarlyRegTypeId = itemConventionInfo.EarlyRegTypeId;
                originConventionInfo.DateInternationalApp = itemConventionInfo.DateInternationalApp;
                originConventionInfo.HeadIps = itemConventionInfo.HeadIps;
                originConventionInfo.RegNumberInternationalApp = itemConventionInfo.RegNumberInternationalApp;
                originConventionInfo.TermNationalPhaseFirsChapter = itemConventionInfo.TermNationalPhaseFirsChapter;
                originConventionInfo.TermNationalPhaseSecondChapter = itemConventionInfo.TermNationalPhaseSecondChapter;
                originConventionInfo.PublishDateInternationalApp = itemConventionInfo.PublishDateInternationalApp;
                originConventionInfo.PublishRegNumberInternationalApp = itemConventionInfo.PublishRegNumberInternationalApp;
                originConventionInfo.DateEurasianApp = itemConventionInfo.DateEurasianApp;
                originConventionInfo.RegNumberEurasianApp = itemConventionInfo.PublishRegNumberEurasianApp;
                originConventionInfo.PublishDateEurasianApp = itemConventionInfo.PublishDateEurasianApp;
                originConventionInfo.PublishRegNumberEurasianApp = itemConventionInfo.PublishRegNumberEurasianApp;
                originConventionInfo.InternationalAppToNationalPhaseTransferDate = itemConventionInfo.InternationalAppToNationalPhaseTransferDate;

                requestConventionInfoRepo.Update(originConventionInfo);
            }

            await Uow.SaveChangesAsync();
        }
    }
}
