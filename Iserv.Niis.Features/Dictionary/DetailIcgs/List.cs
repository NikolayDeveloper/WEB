using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models;
using MediatR;

namespace Iserv.Niis.Features.Dictionary.DetailIcgs
{
    public class List
    {
        public class Query : IRequest<IQueryable<SelectOptionDto>>
        {
            public Query(int icgsId)
            {
                IcgsId = icgsId;
            }

            public int IcgsId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<SelectOptionDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<SelectOptionDto>> Handle(Query message)
            {
                var result = _context.DicDetailICGSs
                    .Where(x => x.IcgsId == message.IcgsId)
                    .ProjectTo<SelectOptionDto>();

                return Task.FromResult(result);
            }
        }
    }
}