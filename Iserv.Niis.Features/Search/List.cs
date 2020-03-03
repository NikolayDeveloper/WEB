using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Search;
using MediatR;

namespace Iserv.Niis.Features.Search
{
    public class List
    {
        public class Query : IRequest<IQueryable<SearchDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<SearchDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<SearchDto>> Handle(Query message)
            {
                return Task.FromResult(_context.SearchViewEntities.ProjectTo<SearchDto>());
            }
        }
    }
}