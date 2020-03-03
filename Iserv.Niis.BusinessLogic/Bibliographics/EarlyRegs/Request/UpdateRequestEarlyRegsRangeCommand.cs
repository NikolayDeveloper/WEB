using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request
{
    public class UpdateRequestEarlyRegsRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<RequestEarlyReg> earlyRegs)
        {
            var requestRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var request = await requestRepository
                .AsQueryable()
                .Include(r => r.EarlyRegs)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null || earlyRegs == null || !earlyRegs.Any())
            {
                return;
            }
            foreach (var earlyReg in earlyRegs)
            {
                var originEarlyReg = request.EarlyRegs.First(x => x.Id == earlyReg.Id);

                originEarlyReg.RegCountryId = earlyReg.RegCountryId;
                originEarlyReg.RegNumber = earlyReg.RegNumber;
                originEarlyReg.RegDate = earlyReg.RegDate;
                originEarlyReg.EarlyRegTypeId = earlyReg.EarlyRegTypeId;

                originEarlyReg.ChapterOne = earlyReg.ChapterOne;
                originEarlyReg.DateOfChapterOne = earlyReg.DateOfChapterOne;
                originEarlyReg.ChapterTwo = earlyReg.ChapterTwo;
                originEarlyReg.DateOfChapterTwo = earlyReg.DateOfChapterTwo;
            }

            await Uow.SaveChangesAsync();
        }
    }
}
