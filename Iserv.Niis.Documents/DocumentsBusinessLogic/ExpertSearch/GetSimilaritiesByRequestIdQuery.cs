using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.ExpertSearch
{
    public class GetSimilaritiesByRequestIdQuery : BaseQuery
    {
        public List<ExpertSearchSimilar> Execute(int requestId)
        {
            var similarityRepository = Uow.GetRepository<ExpertSearchSimilar>();
            var similarities = similarityRepository.AsQueryable()
                .Include(x => x.SimilarProtectionDoc).ThenInclude(p => p.Request).ThenInclude(sr => sr.EarlyRegs).ThenInclude(er => er.RegCountry)
                .Include(x => x.SimilarProtectionDoc).ThenInclude(sp => sp.Type)
                .Include(x => x.SimilarProtectionDoc).ThenInclude(sp => sp.Bulletins).ThenInclude(b => b.Bulletin)
                .Include(x => x.SimilarProtectionDoc).ThenInclude(x => x.Type)
                .Include(x => x.SimilarProtectionDoc).ThenInclude(x => x.SpeciesTradeMark)
                .Include(x => x.SimilarProtectionDoc).ThenInclude(x => x.ProtectionDocCustomers).ThenInclude(rc => rc.Customer)
                .Include(x => x.SimilarProtectionDoc).ThenInclude(x => x.IcgsProtectionDocs).ThenInclude(x => x.Icgs)
                .Include(x => x.SimilarRequest).ThenInclude(x => x.RequestCustomers).ThenInclude(rc => rc.Customer)
                .Include(x => x.SimilarRequest).ThenInclude(x => x.ICGSRequests).ThenInclude(x => x.Icgs)
                .Include(x => x.SimilarRequest).ThenInclude(x => x.ProtectionDocType)
                .Include(x => x.SimilarRequest).ThenInclude(x => x.SpeciesTradeMark)
                .Where(s => s.RequestId == requestId);

            return similarities.ToList();
        }
    }
}