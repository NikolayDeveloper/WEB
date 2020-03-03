using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Model.Models.Material;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Materials
{
    public class WorkflowCreate
    {
        public class Command : IRequest<MaterialWorkflowDto>
        {
            public Command(MaterialWorkflowDto documentWorkflowDto, int userId)
            {
                DocumentWorkflowDto = documentWorkflowDto;
                UserId = userId;
            }

            public MaterialWorkflowDto DocumentWorkflowDto { get; }
            public int UserId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DocumentWorkflowDto.OwnerId).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, MaterialWorkflowDto>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;
            private readonly IWorkflowApplier<Document> _workflowApplier;
            private readonly ITransferedDocumentApplier _transferedDocumentApplier;

            public CommandHandler(
                IMapper mapper,
                NiisWebContext context,
                IWorkflowApplier<Document> workflowApplier,
                ITransferedDocumentApplier transferedDocumentApplier)
            {
                _mapper = mapper;
                _context = context;
                _workflowApplier = workflowApplier;
                _transferedDocumentApplier = transferedDocumentApplier;
            }

            public async Task<MaterialWorkflowDto> Handle(Command message)
            {
                var workflowDto = message.DocumentWorkflowDto;
                workflowDto.CurrentStageCode = _context.DicRouteStages
                    .Where(rs => rs.Id == workflowDto.CurrentStageId)
                    .Select(rs => rs.Code).SingleOrDefault();
                var documentWorkflow = _mapper.Map<MaterialWorkflowDto, DocumentWorkflow>(workflowDto);

                await _workflowApplier.ApplyAsync(documentWorkflow);

                var document = await _context.Documents
                    .Include(d => d.Type)
                    .SingleOrDefaultAsync(d => d.Id == documentWorkflow.OwnerId);

                await _context.SaveChangesAsync();
                //await _contractWorkflowApplier.AutoApplyAsync(document);
                await _transferedDocumentApplier.ApplyAsync(documentWorkflow.OwnerId);
                await _context.SaveChangesAsync();

                var workflow = await _context.DocumentWorkflows
                    .Include(w => w.FromStage)
                    .Include(w => w.CurrentStage)
                    .Include(w => w.FromUser)
                    .Include(w => w.CurrentUser)
                    .Include(w => w.Route)
                    .Include(w => w.Owner)
                    .SingleAsync(w => w.Id == documentWorkflow.Id);
                workflowDto = _mapper.Map<DocumentWorkflow, MaterialWorkflowDto>(workflow);
                if (document != null)
                {
                    workflowDto.OutgoingNum = document.OutgoingNumber;
                    workflowDto.DocumentDate = document.SendingDate;
                }

                return workflowDto;
            }
        }
    }
}
