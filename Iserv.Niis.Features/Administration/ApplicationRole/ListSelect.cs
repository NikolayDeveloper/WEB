using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Administration.ApplicationRole
{
    public class ListSelect
    {
        public class Query : IRequest<IQueryable<SelectOptionDto>> { }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<SelectOptionDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<SelectOptionDto>> Handle(Query message)
            {

                var query = _context.Roles
                    .AsNoTracking()
                    .ProjectTo<SelectOptionDto>();

                return Task.FromResult(query);
            }
        }
    }
}
