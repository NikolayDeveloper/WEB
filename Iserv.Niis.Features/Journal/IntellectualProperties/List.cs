using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Features.Services.Resources;
using Iserv.Niis.Model.Models.Journal;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Journal.IntellectualProperties
{
    public class List
    {
        public class Query : IRequest<IQueryable<IntellectualPropertyDto>>
        {
            public int UserId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<IntellectualPropertyDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            async Task<IQueryable<IntellectualPropertyDto>> IAsyncRequestHandler<Query, IQueryable<IntellectualPropertyDto>>.Handle(Query message)
            {
                var roleStageIds = await RequestRestrictions.GetUserRoleStagesIds(_context, message.UserId);

                var requests = _context.Requests.WherePermissions(message.UserId, roleStageIds);
                var contracts = _context.Contracts;
                var protectionDocs = _context.ProtectionDocs.Include(pd => pd.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage);

                var result = requests.ProjectTo<IntellectualPropertyDto>()
                    .Concat(contracts.ProjectTo<IntellectualPropertyDto>())
                    .Concat(protectionDocs.ProjectTo<IntellectualPropertyDto>());
                return result;
            }
        }
    }
}