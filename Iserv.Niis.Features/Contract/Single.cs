using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Contract;
using Iserv.Niis.Model.Models.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Contract
{
    public class Single
    {
        public class Query : IRequest<ContractDetailDto>
        {
            public Query(int userId, Expression<Func<Domain.Entities.Contract.Contract, bool>> predicate)
            {
                UserId = userId;
                Predicate = predicate;
            }

            public int UserId { get; }
            public Expression<Func<Domain.Entities.Contract.Contract, bool>> Predicate { get; }
        }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, ContractDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(IMapper mapper, NiisWebContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            async Task<ContractDetailDto> IAsyncRequestHandler<Query, ContractDetailDto>.Handle(Query message)
            {
                var contract = await _context.Contracts
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
                    .SingleOrDefaultAsync(message.Predicate);
                if (contract == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Contract.Contract),
                        DataNotFoundException.OperationType.Read, message.Predicate.ToString());

                if (!contract.IsRead && contract.CurrentWorkflow.CurrentUserId == message.UserId)
                {
                    contract.IsRead = true;
                    _context.Contracts.Update(contract);
                    await _context.SaveChangesAsync();
                }

                return _mapper.Map<Domain.Entities.Contract.Contract, ContractDetailDto>(contract,
                    opt => opt.Items["ContractCustomers"] = contract.ContractCustomers);
            }
        }
    }
}