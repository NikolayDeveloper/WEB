using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models;
using MediatR;

namespace Iserv.Niis.Features.Dictionary.DicTariff
{
    public class List
    {
        public class Query : IRequest<IQueryable<SelectOptionDto>>
        {
            public Query(int protectionDocTypeId)
            {
                ProtectionDocTypeId = protectionDocTypeId;
            }

            public int ProtectionDocTypeId { get; set; }
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
                var result = _context.DicTariffs
                    .Where(t => t.ProtectionDocTypeId == message.ProtectionDocTypeId || t.ProtectionDocTypeId == null)
                    .ProjectTo<SelectOptionDto>();
                return Task.FromResult(result);
            }
        }
    }
}