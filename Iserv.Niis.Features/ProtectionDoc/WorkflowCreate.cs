using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.ProtectionDoc
{
    public class WorkflowCreate
    {
        public class Command : IRequest<object>
        {
            public Command(int[] ids, int userId)
            {
                Ids = ids;
                UserId = userId;
            }

            public int[] Ids { get; }
            public int UserId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Ids.Length > 0);
                RuleFor(c => c.UserId != 0);
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, object>
        {
            private readonly NiisWebContext _context;
            private readonly IWorkflowApplier<Domain.Entities.ProtectionDoc.ProtectionDoc> _workflowApplier;

            public CommandHandler(NiisWebContext context,
                IWorkflowApplier<Domain.Entities.ProtectionDoc.ProtectionDoc> workflowApplier)
            {
                _context = context;
                _workflowApplier = workflowApplier;
            }

            public async Task<object> Handle(Command message)
            {
                foreach (var id in message.Ids)
                {
                    var protectionDoc = _context.ProtectionDocs
                        .Include(pd => pd.CurrentWorkflow)
                        .Single(pd => pd.Id == id);
                    var nextStage = _context.DicRouteStages.Single(rs => rs.Code.Equals("OD01.3"));
                    var nextWorkflow = new ProtectionDocWorkflow
                    {
                        OwnerId = protectionDoc.Id,
                        FromUserId = protectionDoc.CurrentWorkflow.CurrentUserId,
                        FromStageId = protectionDoc.CurrentWorkflow.CurrentStageId,
                        CurrentUserId = message.UserId,
                        CurrentStageId = nextStage.Id,
                        RouteId = nextStage.RouteId,
                        IsComplete = nextStage.IsLast,
                        IsSystem = nextStage.IsSystem,
                        IsMain = nextStage.IsMain
                    };
                    await _workflowApplier.ApplyAsync(nextWorkflow);
                }
                return null;
            }
        }
    }
}
