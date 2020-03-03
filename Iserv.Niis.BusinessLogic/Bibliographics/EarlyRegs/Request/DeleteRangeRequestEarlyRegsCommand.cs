using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request
{
    public class DeleteRangeRequestEarlyRegsCommand : BaseCommand
    {
        public async Task ExecuteAsync(List<RequestEarlyReg> earlyRegs)
        {
            var earlyRegRepo = Uow.GetRepository<RequestEarlyReg>();
            earlyRegRepo.DeleteRange(earlyRegs);
            await Uow.SaveChangesAsync();
        }
    }
}