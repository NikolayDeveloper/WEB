using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Material.Incoming;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Materials
{
    public class List
    {
        public class Query : IRequest<IQueryable<MaterialTaskDto>>
        {
            public Query()
            {
            }
        }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<MaterialTaskDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<MaterialTaskDto>> Handle(Query message)
            {
                var documents = _context.Documents
                    .Include(d => d.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                    .Include(d => d.Workflows).ThenInclude(w => w.CurrentUser)
                    .Include(d => d.Type)
                    .Where(
                        d => !d.IsDeleted
                             && (!d.Requests.Any() && d.DocumentType == DocumentType.Incoming
                                 || d.Requests.Any() && d.DocumentType == DocumentType.Outgoing &&
                                 !d.CurrentWorkflow.CurrentStage.Code.Equals("OUT03.1")
                                 || d.DocumentType == DocumentType.Internal && !d.Requests.Any())
                    ).ProjectTo<MaterialTaskDto>();

                return Task.FromResult(documents);
            }
        }
    }
}
