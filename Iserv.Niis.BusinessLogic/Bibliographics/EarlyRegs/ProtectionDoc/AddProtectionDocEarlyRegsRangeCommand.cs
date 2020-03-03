using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.ProtectionDoc
{
    public class AddProtectionDocEarlyRegsRangeCommand: BaseCommand
    {
        private const string EarlyRegTypePriorityDataCode = "30 - 300";
        public async Task ExecuteAsync(int protectionDocId, List<ProtectionDocEarlyReg> earlyRegs)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            foreach (var earlyReg in earlyRegs)
            {
                protectionDoc.EarlyRegs.Add(earlyReg);
            }
            await Uow.SaveChangesAsync();
        }
    }
}
