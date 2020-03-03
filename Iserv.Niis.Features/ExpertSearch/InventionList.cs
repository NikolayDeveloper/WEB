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
    public class InventionList
    {
        public class Query : IRequest<IQueryable<InventionDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<InventionDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<InventionDto>> Handle(Query message)
            {
                var inventionTypeIds = _context.DicProtectionDocTypes.Where(t => new[] { "B", "A4", "A"}.Contains(t.Code))
                    .Select(t => t.Id).ToArray();

                var inventionViewEntities = _context.ExpertSearchViewEntities
                    .Include(t => t.Request).ThenInclude(r => r.EarlyRegs)
                    .Include(t => t.ProtectionDoc).ThenInclude(pd => pd.EarlyRegs)
                    .Where(t => inventionTypeIds.Contains(t.ProtectionDocTypeId));
                
                return Task.FromResult(inventionViewEntities.ProjectTo<InventionDto>());
            }
        }
    }
}