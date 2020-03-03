using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Search
{
    public class GetExpertSearchSimilarsByRequestIdQuery : BaseQuery
    {
        public async Task<List<ExpertSearchSimilar>> Execute(int requestId)
        {
            var repo = Uow.GetRepository<ExpertSearchSimilar>();

            return await repo.AsQueryable()
                .Include(ess => ess.Request).ThenInclude(r => r.EarlyRegs)
                .Include(ess => ess.SimilarRequest).ThenInclude(r => r.EarlyRegs)
                .Include(ess => ess.SimilarRequest).ThenInclude(r => r.Status)
                .Include(ess => ess.SimilarProtectionDoc).ThenInclude(pd => pd.Request)
                .Include(ess => ess.SimilarProtectionDoc).ThenInclude(pd => pd.EarlyRegs)
                .Include(ess => ess.SimilarRequest.ICGSRequests).ThenInclude(i => i.Icgs)
                .Include(ess => ess.SimilarProtectionDoc.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
                .Include(ess => ess.SimilarRequest.Icfems).ThenInclude(i => i.DicIcfem)
                .Include(ess => ess.SimilarProtectionDoc.Icfems).ThenInclude(i => i.DicIcfem)
                .Where(ess => ess.RequestId == requestId)
                .ToListAsync();
        }
    }
}