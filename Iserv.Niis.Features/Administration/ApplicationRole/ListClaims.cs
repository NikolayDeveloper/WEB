using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationRole
{
    public class ListClaims
    {
        public class Query : IRequest<IQueryable<ClaimDto>> { }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<ClaimDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public async Task<IQueryable<ClaimDto>> Handle(Query message)
            {
                var query = _context.ClaimConstants
                    .OrderBy(x => x.NameRu)
                    .AsNoTracking()
                    .ProjectTo<ClaimDto>();

                return await Task.FromResult(query);
            }
        }
    }
}
