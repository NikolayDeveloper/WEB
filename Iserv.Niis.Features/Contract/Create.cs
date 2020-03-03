using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Contract
{
    public class Create
    {
        public class Command : IRequest<ContractDetailDto>
        {
            public Command(ContractDetailDto contractDetailDto, int userId)
            {
                ContractDetailDto = contractDetailDto;
                UserId = userId;
            }

            public ContractDetailDto ContractDetailDto { get; }
            public int UserId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.ContractDetailDto.Id).Equal(0);
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, ContractDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly INumberGenerator _numberGenerator;
            private readonly IWorkflowApplier<Domain.Entities.Contract.Contract> _workflowApplier;
            private readonly IContractRequestRelationUpdater _requestRelationUpdater;

            public CommandHandler(
                IMapper mapper,
                NiisWebContext context,
                INumberGenerator numberGenerator,
                IWorkflowApplier<Domain.Entities.Contract.Contract> workflowApplier,
                IContractRequestRelationUpdater requestRelationUpdater)
            {
                _mapper = mapper;
                _context = context;
                _numberGenerator = numberGenerator;
                _workflowApplier = workflowApplier;
                _requestRelationUpdater = requestRelationUpdater;
            }

            public async Task<ContractDetailDto> Handle(Command message)
            {
                var contractDto = message.ContractDetailDto;
                var contract = _mapper.Map<ContractDetailDto, Domain.Entities.Contract.Contract>(contractDto);
                contract.RegDate = DateTimeOffset.Now;
                contract.ContractNum = _numberGenerator.Generate("ContractNum").ToString();
                _numberGenerator.GenerateBarcode(contract);

                _context.Contracts.Add(contract);

                try
                {
                    await _context.SaveChangesAsync();
                    
                    await _workflowApplier.ApplyInitialAsync(contract, message.UserId);
                    await _context.SaveChangesAsync();

                    await _requestRelationUpdater.UpdateAsync(contract.Id, contractDto.RequestRelations);

                    var contractWithIncludes = await _context.Contracts
                        .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                        .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                        .Include(r => r.ProtectionDocType)
                        .Include(r => r.ContractType)
                        .Include(r => r.ContractCustomers).ThenInclude(c => c.CustomerRole)
                        .Include(r => r.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                        .Include(r => r.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                        .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                        .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                        .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentStage)
                        .Include(r => r.Workflows).ThenInclude(r => r.FromUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.CurrentUser)
                        .Include(r => r.Workflows).ThenInclude(r => r.Route)
                        .Include(r => r.RequestsForProtectionDoc).ThenInclude(r => r.Request).ThenInclude(r => r.ProtectionDocType)
                        .Include(r => r.RequestsForProtectionDoc).ThenInclude(r => r.ContractRequestICGSRequests).ThenInclude(r => r.ICGSRequest).ThenInclude(r => r.Icgs)
                        .Include(r => r.ProtectionDocs)
                        .SingleAsync(c => c.Id == contract.Id);
                    return _mapper.Map<Domain.Entities.Contract.Contract, ContractDetailDto>(contractWithIncludes);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }
        }
    }
}