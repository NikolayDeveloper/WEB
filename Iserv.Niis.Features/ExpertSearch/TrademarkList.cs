using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.ExpertSearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ExpertSearch
{
    public class TrademarkList
    {
        public class Query : IRequest<IQueryable<TrademarkDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<TrademarkDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<TrademarkDto>> Handle(Query message)
            {
                var trademarkTypeIds = _context.DicProtectionDocTypes.Where(t => new[] {"TM", "ITM"}.Contains(t.Code))
                    .Select(t => t.Id).ToArray();

                var trademarkViewEntities = _context.ExpertSearchViewEntities
                    .Include(t => t.Request).ThenInclude(r => r.EarlyRegs)
                    .Include(t => t.ProtectionDoc).ThenInclude(pd => pd.EarlyRegs)
                    .Where(t => trademarkTypeIds.Contains(t.ProtectionDocTypeId));
                
                return Task.FromResult(trademarkViewEntities.ProjectTo<TrademarkDto>());
            }
        }
    }
}