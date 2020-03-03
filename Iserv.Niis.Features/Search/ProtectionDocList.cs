using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Search;
using MediatR;

namespace Iserv.Niis.Features.Search
{
    public class ProtectionDocList
    {
        public class Query : IRequest<IQueryable<ProtectionDocSearchDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<ProtectionDocSearchDto>>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(NiisWebContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public Task<IQueryable<ProtectionDocSearchDto>> Handle(Query message)
            {
                return Task.FromResult(_mapper.Map<IQueryable<ProtectionDocSearchDto>>(_context.ProtectionDocs));
            }
        }
    }
}