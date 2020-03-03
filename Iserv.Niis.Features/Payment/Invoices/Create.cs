using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Payment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Payment.Invoices
{
    public class Create
    {
        public class Command : IRequest<PaymentInvoiceDto>
        {
            public Command(PaymentInvoiceDto invoiceDto, Owner.Type ownerType)
            {
                PaymentInvoiceDto = invoiceDto;
                OwnerType = ownerType;
            }

            public PaymentInvoiceDto PaymentInvoiceDto { get; }
            public Owner.Type OwnerType { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PaymentInvoiceDto.Id).Equal(0);
                RuleFor(c => c.PaymentInvoiceDto.OwnerId).NotEmpty();
                RuleFor(c => c.PaymentInvoiceDto.TariffId).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, PaymentInvoiceDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(IMapper mapper, NiisWebContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<PaymentInvoiceDto> Handle(Command message)
            {
                var invoiceDto = message.PaymentInvoiceDto;
                var paymentInvoice = _mapper.Map<PaymentInvoiceDto, PaymentInvoice>(invoiceDto, opt => opt.Items["OwnerType"] = message.OwnerType);

                _context.PaymentInvoices.Add(paymentInvoice);

                try
                {
                    await _context.SaveChangesAsync();

                    var invoice = _context.PaymentInvoices
                        .Include(pi => pi.PaymentUses)
                        .Include(pi => pi.Tariff)
                        .Include(pi => pi.Status)
                        .Single(pi => pi.Id == paymentInvoice.Id);

                    switch (message.OwnerType)
                    {
                        case Owner.Type.Request:
                        {
                            var request = _context.Requests
                                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Request).ThenInclude(c => c.ProtectionDocType)
                                .Include(r => r.RequestCustomers).ThenInclude(r => r.CustomerRole)
                                .Include(r => r.RequestCustomers).ThenInclude(r => r.Customer)
                                .ThenInclude(c => c.Type)
                                .Include(r => r.ProtectionDocType)
                                .Single(r => r.Id == paymentInvoice.RequestId);
                            return _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(invoice,
                                opt => opt.Items["RequestCustomers"] = request.RequestCustomers);
                        }
                        case Owner.Type.Contract:
                        {
                            var contract = _context.Contracts
                                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Contract).ThenInclude(c => c.ProtectionDocType)
                                .Include(r => r.ContractCustomers).ThenInclude(r => r.CustomerRole)
                                .Include(r => r.ContractCustomers).ThenInclude(r => r.Customer).ThenInclude(c => c.Type)
                                .Single(r => r.Id == paymentInvoice.ContractId);
                            return _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(invoice,
                                opt => opt.Items["ContractCustomers"] = contract.ContractCustomers);
                        }
                        case Owner.Type.ProtectionDoc:
                        {
                            var protectionDoc = _context.ProtectionDocs
                                .Include(pd => pd.PaymentInvoices).ThenInclude(pi => pi.Contract).ThenInclude(c => c.ProtectionDocType)
                                .Include(pd => pd.ProtectionDocCustomers).ThenInclude(r => r.CustomerRole)
                                .Include(pd => pd.ProtectionDocCustomers).ThenInclude(r => r.Customer)
                                .ThenInclude(c => c.Type)
                                .Single(pd => pd.Id == paymentInvoice.ProtectionDocId);
                            return _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(invoice,
                                opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers);
                        }
                        default:
                            throw new ApplicationException(string.Empty,
                                new ArgumentException($"{nameof(message.OwnerType)}: {message.OwnerType}"));
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