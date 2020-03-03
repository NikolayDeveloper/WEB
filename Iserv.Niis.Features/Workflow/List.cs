using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Model.Models.Subject;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Workflow
{
    public class List
    {
        public class Query : IRequest<IQueryable<WorkflowDto>>
        {
            public Query(int ownerId, Owner.Type ownerType)
            {
                OwnerId = ownerId;
                OwnerType = ownerType;
            }

            public int OwnerId { get; }
            public Owner.Type OwnerType { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<WorkflowDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public Task<IQueryable<WorkflowDto>> Handle(Query message)
            {
                var ownerId = message.OwnerId;
                var ownerType = message.OwnerType;

                switch (ownerType)
                {
                    case Owner.Type.Request:
                        var requestWorkflows = _context.RequestWorkflows
                            .Include(r => r.FromStage)
                            .Include(r => r.CurrentStage)
                            .Include(r => r.FromUser)
                            .Include(r => r.CurrentUser)
                            .Include(r => r.Route)
                            .Where(rc => rc.OwnerId == ownerId)
                            .OrderByDescending(rc => rc.DateCreate);

                        return Task.FromResult(requestWorkflows.ProjectTo<WorkflowDto>());
                    case Owner.Type.ProtectionDoc:
                        requestWorkflows = _context.RequestWorkflows
                            .Include(r => r.FromStage)
                            .Include(r => r.CurrentStage)
                            .Include(r => r.FromUser)
                            .Include(r => r.CurrentUser)
                            .Include(r => r.Route)
                            .Where(r => r.Owner.ProtectionDocId == ownerId)
                            .OrderByDescending(rc => rc.DateCreate);
                        var protectionDocWorkflows = _context.ProtectionDocWorkflows
                            .Include(r => r.FromStage)
                            .Include(r => r.CurrentStage)
                            .Include(r => r.FromUser)
                            .Include(r => r.CurrentUser)
                            .Include(r => r.Route)
                            .Where(rc => rc.OwnerId == ownerId);
                        return Task.FromResult(requestWorkflows.ProjectTo<WorkflowDto>()
                            .Concat(protectionDocWorkflows.ProjectTo<WorkflowDto>())
                            .OrderByDescending(w => w.DateCreate).AsQueryable());
                    case Owner.Type.Contract:
                        var contractWorkflows = _context.ContractWorkflows
                            .Include(r => r.FromStage)
                            .Include(r => r.CurrentStage)
                            .Include(r => r.FromUser)
                            .Include(r => r.CurrentUser)
                            .Include(r => r.Route)
                            .Where(rc => rc.OwnerId == ownerId)
                            .OrderByDescending(rc => rc.DateCreate);

                        return Task.FromResult(contractWorkflows.ProjectTo<WorkflowDto>());
                    default:
                        throw new ApplicationException(string.Empty,
                            new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
                }
            }
        }
    }
}