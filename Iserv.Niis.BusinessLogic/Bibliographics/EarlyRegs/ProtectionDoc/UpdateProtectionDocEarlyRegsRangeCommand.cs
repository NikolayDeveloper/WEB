using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.ProtectionDoc
{
    public class UpdateProtectionDocEarlyRegsRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<ProtectionDocEarlyReg> earlyRegs)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);

            if (protectionDoc == null || earlyRegs == null || !earlyRegs.Any())
            {
                return;
            }

            foreach (var earlyReg in earlyRegs)
            {
                var originEarlyReg = protectionDoc.EarlyRegs.First(x => x.Id == earlyReg.Id);
                originEarlyReg.RegCountryId = earlyReg.RegCountryId;
                originEarlyReg.RegNumber = earlyReg.RegNumber;
                originEarlyReg.RegDate = earlyReg.RegDate;
                originEarlyReg.EarlyRegTypeId = earlyReg.EarlyRegTypeId;
            }

            await Uow.SaveChangesAsync();
        }
    }
}
