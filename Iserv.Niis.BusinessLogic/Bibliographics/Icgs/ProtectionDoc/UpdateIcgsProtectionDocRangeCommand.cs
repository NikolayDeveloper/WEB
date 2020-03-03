using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.ProtectionDoc
{
    public class UpdateIcgsProtectionDocRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<ICGSProtectionDoc> icgsProtectionDocs)
        {
            var repo = Uow.GetRepository<ICGSProtectionDoc>();

            foreach (var icgs in icgsProtectionDocs)
            {
                var originIcgs = repo.GetById(icgs.Id);
                originIcgs.ClaimedDescription = icgs.ClaimedDescription;
                originIcgs.Description = icgs.Description;
                originIcgs.DescriptionKz = icgs.DescriptionKz;
                originIcgs.NegativeDescription = icgs.NegativeDescription;
                originIcgs.IcgsId = icgs.IcgsId;
                originIcgs.ProtectionDocId = icgs.ProtectionDocId;
                repo.Update(originIcgs);
            }

            await Uow.SaveChangesAsync();
        }
    }
}
