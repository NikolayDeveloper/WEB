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
using Iserv.Niis.Workflow.Abstract;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Payment.Uses
{
    public class Create
    {
        public class Command : IRequest<PaymentUseDto>
        {
            public Command(PaymentUseDto useDto, Owner.Type ownerType)
            {
                UseDto = useDto;
                OwnerType = ownerType;
            }

            public PaymentUseDto UseDto { get; }
            public Owner.Type OwnerType { get; }
        }

        public class CommandValidator : AbstractValidator<Create.Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.UseDto.Amount).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Create.Command, PaymentUseDto>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;
            private readonly IPaymentApplier<Domain.Entities.Request.Request> _requestPaymentApplier;
            private readonly IPaymentApplier<Domain.Entities.Contract.Contract> _contractPaymentApplier;

            public CommandHandler(IMapper mapper, 
                NiisWebContext context, 
                IPaymentApplier<Domain.Entities.Request.Request> requestPaymentApplier, 
                IPaymentApplier<Domain.Entities.Contract.Contract> contractPaymentApplier)
            {
                _mapper = mapper;
                _context = context;
                _requestPaymentApplier = requestPaymentApplier;
                _contractPaymentApplier = contractPaymentApplier;
            }

            public async Task<PaymentUseDto> Handle(Create.Command message)
            {
                var useDto = message.UseDto;
                var paymentUse = _mapper.Map<PaymentUseDto, PaymentUse>(useDto);

                _context.PaymentUses.Add(paymentUse);

                if (useDto.PaymentInvoiceStatusId.HasValue)
                {
                    var paymentInvoice = _context.PaymentInvoices.SingleOrDefault(pi => pi.Id == useDto.PaymentInvoiceId);
                    if (paymentInvoice != null) paymentInvoice.StatusId = (int)useDto.PaymentInvoiceStatusId;
                }

                await _context.SaveChangesAsync();

                switch (message.OwnerType)
                {
                    case Owner.Type.Request:
                        await _requestPaymentApplier.ApplyAsync(paymentUse.Id);
                        break;
                    case Owner.Type.Contract:
                        await _contractPaymentApplier.ApplyAsync(paymentUse.Id);
                        break;
                }
                
                try
                {
                    await _context.SaveChangesAsync();
                    
                    return _mapper.Map<PaymentUse, PaymentUseDto>(paymentUse);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }
        }
    }
}