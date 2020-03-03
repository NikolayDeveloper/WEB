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
    public class Register
    {
        public class Command : IRequest<ContractDetailDto>
        {
            public Command(int contractId, ContractDetailDto contractDetailDto, int userId)
            {
                ContractDetailDto = contractDetailDto;
                ContractId = contractId;
                UserId = userId;
            }

            public ContractDetailDto ContractDetailDto { get; }
            public int ContractId { get; }
            public int UserId { get; }
        }

        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator()
            {
                
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, ContractDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly IWorkflowApplier<Domain.Entities.Contract.Contract> _workflowApplier;
            private readonly INumberGenerator _numberGenerator;
            public CommandHandler(IWorkflowApplier<Domain.Entities.Contract.Contract> workflowApplier, IMapper mapper, NiisWebContext context, INumberGenerator numberGenerator)
            {
                _workflowApplier = workflowApplier;
                _mapper = mapper;
                _context = context;
                _numberGenerator = numberGenerator;
            }

            public async Task<ContractDetailDto> Handle(Command message)
            {

                var contractId = message.ContractDetailDto.Id = message.ContractId;
                var contractDto = message.ContractDetailDto;

                var contract = await _context.Contracts
                    .Include(r => r.ProtectionDocType)
                    .Include(r => r.ContractType)
                    .Include(r => r.Category)
                    .SingleOrDefaultAsync(r => r.Id == contractId);
                if (contract == null)
                {
                    throw new DataNotFoundException(nameof(Domain.Entities.Contract.Contract),
                        DataNotFoundException.OperationType.Update, contractId);
                }
                _mapper.Map(contractDto, contract);
                _numberGenerator.GenerateGosNumber(contract);

                try
                {
                    await _context.SaveChangesAsync();

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
                        .SingleAsync(r => r.Id == contractId);
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