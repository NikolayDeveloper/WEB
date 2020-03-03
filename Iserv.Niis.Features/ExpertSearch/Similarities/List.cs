using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models.ExpertSearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ExpertSearch.Similarities
{
	public class List
	{
		public class Query : IRequest<IQueryable<TrademarkDto>>
		{
			public Query(int requestId)
			{
				RequestId = requestId;
			}

			public int RequestId { get; }
		}

		public class QueryValidator : AbstractValidator<Query>
		{
		}

		public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<TrademarkDto>>
		{
			private readonly NiisWebContext _context;
			private readonly IMapper _mapper;
			private readonly IFileStorage _fileStorage;

			public QueryHandler(NiisWebContext context, IMapper mapper, IFileStorage fileStorage)
			{
				_context = context;
				_mapper = mapper;
				_fileStorage = fileStorage;
			}

			public async Task<IQueryable<TrademarkDto>> Handle(Query message)
			{
			    var expertSearchSimilars = _context.ExpertSearchSimilarities
			        .Include(p => p.Request).ThenInclude(er => er.EarlyRegs)
			        .Include(r => r.SimilarRequest).ThenInclude(pd => pd.EarlyRegs)
			        .Include(r => r.SimilarProtectionDoc).ThenInclude(pd => pd.Request)
			        .Include(r => r.SimilarProtectionDoc).ThenInclude(pd => pd.EarlyRegs)
			        .Include(pd => pd.SimilarRequest.ICGSRequests).ThenInclude(i => i.Icgs)
			        .Include(pd => pd.SimilarProtectionDoc.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
			        .Include(pd => pd.SimilarRequest.Icfems).ThenInclude(i => i.DicIcfem)
			        .Include(pd => pd.SimilarProtectionDoc.Icfems).ThenInclude(i => i.DicIcfem)
			        .Where(esr => esr.RequestId == message.RequestId);

			    var requestDtos = expertSearchSimilars
			        .Where(s => s.OwnerType == Owner.Type.Request && s.SimilarRequestId.HasValue)
                    .Select(s => s.SimilarRequest)
			        .ProjectTo<TrademarkDto>()
			        .ToList()
                    .Join(expertSearchSimilars, d => d.Id, s => s.SimilarRequestId,
                        (d, s) =>
                        {
                            d.ImageSimilarity = s.ImageSimilarity;
                            d.PhonSimilarity = s.PhonSimilarity;
                            d.SemSimilarity = s.SemSimilarity;
                            d.ProtectionDocFormula = s.ProtectionDocFormula;
                            d.ProtectionDocCategory = s.ProtectionDocCategory;
                            d.Id = s.Id;
                            return d;
                        });

                var protectionDocDtos = expertSearchSimilars
                    .Where(s => s.OwnerType == Owner.Type.ProtectionDoc && s.SimilarProtectionDocId.HasValue)
                    .Select(r => r.SimilarProtectionDoc)
                    .ProjectTo<TrademarkDto>()
                    .ToList()
                    .Join(expertSearchSimilars, d => d.Id, s => s.SimilarProtectionDocId,
                        (d, s) =>
                        {
                            d.ImageSimilarity = s.ImageSimilarity;
                            d.PhonSimilarity = s.PhonSimilarity;
                            d.SemSimilarity = s.SemSimilarity;
                            d.ProtectionDocFormula = s.ProtectionDocFormula;
                            d.ProtectionDocCategory = s.ProtectionDocCategory;
                            d.Id = s.Id;
                            return d;
                        });

                return await Task.FromResult(requestDtos.Concat(protectionDocDtos).AsQueryable());
			}
		}
	}
}
