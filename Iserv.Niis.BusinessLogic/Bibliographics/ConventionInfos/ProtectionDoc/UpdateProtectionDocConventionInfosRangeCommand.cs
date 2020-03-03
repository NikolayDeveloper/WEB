using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc
{
    public class UpdateProtectionDocConventionInfosRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<ProtectionDocConventionInfo> infos)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);

            if (protectionDoc == null || infos == null || !infos.Any())
            {
                return;
            }
            foreach (var itemConventionInfo in infos)
            {
                var originConventionInfo =
                    protectionDoc.ProtectionDocConventionInfos.FirstOrDefault(rc => rc.Id == itemConventionInfo.Id);

                if (originConventionInfo == null)
                {
                    continue;
                }

                originConventionInfo.CountryId = itemConventionInfo.CountryId;
                originConventionInfo.EarlyRegTypeId = itemConventionInfo.EarlyRegTypeId;
                originConventionInfo.DateInternationalApp = itemConventionInfo.DateInternationalApp;
                originConventionInfo.HeadIps = itemConventionInfo.HeadIps;
                originConventionInfo.RegNumberInternationalApp = itemConventionInfo.RegNumberInternationalApp;
                originConventionInfo.TermNationalPhaseFirsChapter = itemConventionInfo.TermNationalPhaseFirsChapter;
                originConventionInfo.TermNationalPhaseSecondChapter = itemConventionInfo.TermNationalPhaseSecondChapter;
            }

            await Uow.SaveChangesAsync();
        }
    }
}
