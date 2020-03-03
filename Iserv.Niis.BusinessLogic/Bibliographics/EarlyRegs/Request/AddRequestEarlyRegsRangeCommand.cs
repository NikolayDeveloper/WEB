using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request
{
    public class AddRequestEarlyRegsRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<RequestEarlyReg> earlyRegs)
        {
            var requestRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var request = await requestRepository.GetByIdAsync(requestId);
            foreach (var earlyReg in earlyRegs)
            {
                earlyReg.RequestId = requestId;
                request.EarlyRegs.Add(earlyReg);
            }
            await Uow.SaveChangesAsync();
        }
    }
}
