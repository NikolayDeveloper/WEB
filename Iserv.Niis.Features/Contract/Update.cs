using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Contract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Contract
{
    public class Update
    {
        public class Command : IRequest<ContractDetailDto>
        {
            public Command(int contractId, ContractDetailDto contractDetailDto)
            {
                ContractId = contractId;
                ContractDetailDto = contractDetailDto;
            }
            public int ContractId { get; }
            public ContractDetailDto ContractDetailDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, ContractDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly IContractRequestRelationUpdater _requestRelationUpdater;

            public CommandHandler(
                NiisWebContext context, 
                IMapper mapper, 
                IContractRequestRelationUpdater requestRelationUpdater)
            {
                _context = context;
                _mapper = mapper;
                _requestRelationUpdater = requestRelationUpdater;
            }

            public async Task<ContractDetailDto> Handle(Command message)
            {
                var contractId = message.ContractDetailDto.Id = message.ContractId;
                var contractDto = message.ContractDetailDto;

                var contract = await _context.Contracts
                    .SingleOrDefaultAsync(r => r.Id == contractId);
                if (contract == null)
                {
                    throw new DataNotFoundException(nameof(Domain.Entities.Contract.Contract),
                        DataNotFoundException.OperationType.Update, contractId);
                }

                _mapper.Map(contractDto, contract);

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {

                        await _context.SaveChangesAsync();

                        await _requestRelationUpdater.UpdateAsync(contract.Id, contractDto.RequestRelations);
                        transaction.Commit();

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
                        transaction.Rollback();
                        throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                    }
                }
            }
        }
    }
}