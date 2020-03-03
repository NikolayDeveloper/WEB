using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Search;
using MediatR;

namespace Iserv.Niis.Features.Search
{
    public class RequestList
    {
        public class Query : IRequest<IQueryable<RequestSearchDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<RequestSearchDto>>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(NiisWebContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<IQueryable<RequestSearchDto>> Handle(Query message)
            {
                return Task.FromResult(_mapper.Map<IQueryable<RequestSearchDto>>(_context.Requests));
            }
        }
    }
}