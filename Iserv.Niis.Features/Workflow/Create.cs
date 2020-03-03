using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Workflow
{
    public class Create
    {
        public class Command : IRequest<WorkflowDto>
        {
            public Command(WorkflowDto requestDetailDto, Owner.Type ownerType)
            {
                WorkflowDto = requestDetailDto;
                OwnerType = ownerType;
            }

            public WorkflowDto WorkflowDto { get; }
            public Owner.Type OwnerType { get; }
        }

        public class CommandValidator : AbstractValidator<Create.Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.WorkflowDto.OwnerId).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Create.Command, WorkflowDto>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;
            private readonly IWorkflowApplier<Domain.Entities.Request.Request> _workflowApplier;
            private readonly IWorkflowApplier<Domain.Entities.Contract.Contract> _contractworkflowApplier;
            private readonly IWorkflowApplier<Domain.Entities.ProtectionDoc.ProtectionDoc> _protectionDocWorkflowApplier;

            public CommandHandler(
                IMapper mapper, 
                NiisWebContext context,
                IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier,
                IWorkflowApplier<Domain.Entities.Contract.Contract> contractworkflowApplier,
                IWorkflowApplier<Domain.Entities.ProtectionDoc.ProtectionDoc> protectionDocWorkflowApplier)
            {
                _mapper = mapper;
                _context = context;
                _workflowApplier = workflowApplier;
                _contractworkflowApplier = contractworkflowApplier;
                _protectionDocWorkflowApplier = protectionDocWorkflowApplier;
            }

            public async Task<WorkflowDto> Handle(Create.Command message)
            {
                var workflowDto = message.WorkflowDto;

                try
                {
                    switch (message.OwnerType)
                    {
                        case Owner.Type.Request:
                        {
                            var requestWorkflow = _mapper.Map<WorkflowDto, RequestWorkflow>(workflowDto);

                            await _workflowApplier.ApplyAsync(requestWorkflow);
                            await _context.SaveChangesAsync();

                            var workflow = await _context.RequestWorkflows
                                .Include(r => r.FromStage)
                                .Include(r => r.CurrentStage)
                                .Include(r => r.FromUser)
                                .Include(r => r.CurrentUser)
                                .Include(r => r.Route)
                                .Include(r => r.Owner)
                                .SingleAsync(w => w.Id == requestWorkflow.Id);
                            return _mapper.Map<RequestWorkflow, WorkflowDto>(workflow);
                        }
                        case Owner.Type.ProtectionDoc:
                            var protectionDocWorkflow = _mapper.Map<WorkflowDto, ProtectionDocWorkflow>(workflowDto);

                            await _protectionDocWorkflowApplier.ApplyAsync(protectionDocWorkflow);
                            await _context.SaveChangesAsync();

                            var newProtectionDocWorkflow = await _context.ProtectionDocWorkflows
                                .Include(r => r.FromStage)
                                .Include(r => r.CurrentStage)
                                .Include(r => r.FromUser)
                                .Include(r => r.CurrentUser)
                                .Include(r => r.Route)
                                .Include(r => r.Owner)
                                .SingleAsync(w => w.Id == protectionDocWorkflow.Id);
                            return _mapper.Map<ProtectionDocWorkflow, WorkflowDto>(newProtectionDocWorkflow);

                        case Owner.Type.Contract:
                        {
                            var contractWorkflow = _mapper.Map<WorkflowDto, ContractWorkflow>(workflowDto);

                            await _contractworkflowApplier.ApplyAsync(contractWorkflow);
                            await _context.SaveChangesAsync();

                            var workflow = await _context.ContractWorkflows
                                .Include(r => r.FromStage)
                                .Include(r => r.CurrentStage)
                                .Include(r => r.FromUser)
                                .Include(r => r.CurrentUser)
                                .Include(r => r.Route)
                                .Include(r => r.Owner)
                                .SingleAsync(w => w.Id == contractWorkflow.Id);
                            return _mapper.Map<ContractWorkflow, WorkflowDto>(workflow);
                            }
                        case Owner.Type.Material:
                            throw new NotImplementedException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }
        }
    }
}