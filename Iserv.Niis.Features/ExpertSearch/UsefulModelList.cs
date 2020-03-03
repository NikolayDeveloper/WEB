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
    public class UsefulModelList
    {
        public class Query : IRequest<IQueryable<UsefulModelDto>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<UsefulModelDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<UsefulModelDto>> Handle(Query message)
            {
                var usefulModelTypeIds = _context.DicProtectionDocTypes.Where(t => new[] { "U" }.Contains(t.Code))
                    .Select(t => t.Id).ToArray();

                var inventionViewEntities = _context.ExpertSearchViewEntities
                    .Include(t => t.Request).ThenInclude(r => r.EarlyRegs)
                    .Include(t => t.ProtectionDoc).ThenInclude(pd => pd.EarlyRegs)
                    .Where(t => usefulModelTypeIds.Contains(t.ProtectionDocTypeId));
                
                return Task.FromResult(inventionViewEntities.ProjectTo<UsefulModelDto>());
            }
        }
    }
}