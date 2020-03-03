using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Model.Models.ExpertSearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Request
{
    public class GetSimilarProtectionDocs
	{
		public class Query : IRequest<IQueryable<TrademarkDto>>
		{
			public Query(int requestId)
			{
				RequestId = requestId;
			}

			public int RequestId { get; set; }
			public int[] ProtectionDocsId { get; set; }
		}

		public class QueryValidator : AbstractValidator<Query>
		{
		}

		public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<TrademarkDto>>
		{
			private readonly NiisWebContext _context;
			private IMapper _mapper;

			public QueryHandler(NiisWebContext context, IMapper mapper)
			{
				_context = context;
				_mapper = mapper;
			}

			public async Task<IQueryable<TrademarkDto>> Handle(Query message)
			{
				var protectionDocSimilarities = await _context.RequestProtectionDocSimilarities
					 .Where(x => x.RequestId == message.RequestId)
					 .ToListAsync();
				var requests = protectionDocSimilarities.Select(pd => pd.ProtectionDoc);

				var imageDtos = new HashSet<TrademarkDto>();
				foreach (var request in requests)
				{
					imageDtos.Add(_mapper.Map<TrademarkDto>(request));
				}

				return imageDtos.AsQueryable();
			}
		}
	}
}
