using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models;
using MediatR;

namespace Iserv.Niis.Features.Materials
{
    public class AvailableTypesList
    {
        public class Query : IRequest<IQueryable<SelectOptionDto>>
        {
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

            public async Task<IQueryable<SelectOptionDto>> Handle(Query message)
            {
                return _context.DicDocumentTypes
                    .Where(x => x.TemplateFileId.HasValue
                                && x.TemplateFile.File != null).ProjectTo<SelectOptionDto>();
            }
        }
    }
}
