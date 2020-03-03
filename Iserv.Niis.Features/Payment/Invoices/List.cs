using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Payment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Payment.Invoices
{
    public class List
    {
        public class Query : IRequest<IQueryable<PaymentInvoiceDto>>
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
            public QueryValidator()
            {
                RuleFor(p => p.OwnerId).NotEqual(0);
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<PaymentInvoiceDto>>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(NiisWebContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            async Task<IQueryable<PaymentInvoiceDto>> IAsyncRequestHandler<Query, IQueryable<PaymentInvoiceDto>>.
                Handle(Query message)
            {
                var requestId = message.OwnerId;

                switch (message.OwnerType)
                {
                    case Owner.Type.Request:
                    {
                        var request = await _context.Requests
                            .Include(r => r.RequestCustomers).ThenInclude(r => r.CustomerRole)
                            .Include(r => r.RequestCustomers).ThenInclude(r => r.Customer).ThenInclude(c => c.Type)
                            .SingleOrDefaultAsync(r => r.Id == requestId);

                        if (request == null)
                            throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Read,
                                message.OwnerId);

                        var invoices = _context.PaymentInvoices
                            .Include(pi => pi.Request).ThenInclude(c => c.ProtectionDocType)
                            .Include(pi => pi.PaymentUses)
                            .Include(pi => pi.Tariff)
                            .Include(pi => pi.Status)
                            .Where(pi => pi.RequestId == requestId)
                            .OrderByDescending(pi => pi.DateCreate);

                        return _mapper.Map<IQueryable<PaymentInvoice>, IEnumerable<PaymentInvoiceDto>>(invoices,
                            opt => opt.Items["RequestCustomers"] = request.RequestCustomers).AsQueryable();
                    }
                    case Owner.Type.Contract:
                    {
                        var contract = await _context.Contracts
                            .Include(c => c.ContractCustomers).ThenInclude(c => c.CustomerRole)
                            .Include(c => c.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                            .SingleOrDefaultAsync(c => c.Id == requestId);

                        if (contract == null)
                        {
                            throw new DataNotFoundException(nameof(Contract), DataNotFoundException.OperationType.Read, message.OwnerId);
                        }

                        var contractInvoices = _context.PaymentInvoices
                            .Include(pi => pi.Contract).ThenInclude(c => c.ProtectionDocType)
                            .Include(pi => pi.PaymentUses)
                            .Include(pi => pi.Tariff)
                            .Include(pi => pi.Status)
                            .Where(pi => pi.ContractId == requestId)
                            .OrderByDescending(pi => pi.DateCreate);

                        return _mapper.Map<IQueryable<PaymentInvoice>, IEnumerable<PaymentInvoiceDto>>(contractInvoices,
                            opt => opt.Items["ContractCustomers"] = contract.ContractCustomers).AsQueryable();
                    }
                    case Owner.Type.ProtectionDoc:
                        var protectionDoc = await _context.ProtectionDocs
                            .Include(c => c.ProtectionDocCustomers).ThenInclude(c => c.CustomerRole)
                            .Include(c => c.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                            .SingleOrDefaultAsync(c => c.Id == requestId);

                        if (protectionDoc == null)
                        {
                            throw new DataNotFoundException(nameof(Domain.Entities.ProtectionDoc.ProtectionDoc), DataNotFoundException.OperationType.Read, message.OwnerId);
                        }

                        var protectionDocInvoices = _context.PaymentInvoices
                            .Include(pi => pi.ProtectionDoc).ThenInclude(c => c.Type)
                            .Include(pi => pi.PaymentUses)
                            .Include(pi => pi.Tariff)
                            .Include(pi => pi.Status)
                            .Where(pi => pi.ProtectionDocId == requestId)
                            .OrderByDescending(pi => pi.DateCreate);

                        return _mapper.Map<IQueryable<PaymentInvoice>, IEnumerable<PaymentInvoiceDto>>(protectionDocInvoices,
                            opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers).AsQueryable();
                    default:
                        throw new ApplicationException(string.Empty,
                            new ArgumentException($"{nameof(message.OwnerType)}: {message.OwnerType}"));
                }

                
            }
        }
    }
}