using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Request;
using MediatR;

namespace Iserv.Niis.Features.Request
{
    public class ICGSRequestListByRequestIds
    {
        public class Query : IRequest<IQueryable<ICGSRequestItemDto>>
        {
            public Query(int[] requestIds)
            {
                RequestIds = requestIds;
            }

            public int[] RequestIds { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<ICGSRequestItemDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            Task<IQueryable<ICGSRequestItemDto>> IAsyncRequestHandler<Query, IQueryable<ICGSRequestItemDto>>.Handle(Query message)
            {
                var icgsRequests = _context.ICGSRequests.Where(ir => message.RequestIds.Contains(ir.RequestId));
                var dtos = icgsRequests.ProjectTo<ICGSRequestItemDto>();
                return Task.FromResult(dtos);
            }
        }
    }
}