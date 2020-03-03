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
    public class IndustrialDesignList
    {
        public class Query : IRequest<IQueryable<IndustrialDesignDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<IndustrialDesignDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<IndustrialDesignDto>> Handle(Query message)
            {
                var industrialDesignTypeIds = _context.DicProtectionDocTypes.Where(t => new[] { "S2", "S1" }.Contains(t.Code))
                    .Select(t => t.Id).ToArray();

                var industrialDesignViewEntities = _context.ExpertSearchViewEntities
                    .Include(t => t.Request).ThenInclude(r => r.EarlyRegs)
                    .Include(t => t.ProtectionDoc).ThenInclude(pd => pd.EarlyRegs)
                    .Where(t => industrialDesignTypeIds.Contains(t.ProtectionDocTypeId));
                
                return Task.FromResult(industrialDesignViewEntities.ProjectTo<IndustrialDesignDto>());
            }
        }
    }
}