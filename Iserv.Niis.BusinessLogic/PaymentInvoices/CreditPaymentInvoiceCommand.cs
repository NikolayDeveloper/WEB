using AutoMapper;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Data;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class CreditPaymentInvoiceCommand : BaseCommand
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public CreditPaymentInvoiceCommand(IMapper mapper, IExecutor executor)
        {
            _mapper = mapper;
            _executor = executor;
        }

        public void Execute(Owner.Type ownerType, PaymentUse paymentUse, bool force)
        {
            if (!paymentUse.PaymentInvoiceId.HasValue)
            {
                return;
            }

            var paymentInvoice = _executor.GetQuery<GetPaymentInvoiceByIdQuery>()
                .Process(q => q.ExecuteAsync(paymentUse.PaymentInvoiceId.Value)).Result;
            if (paymentInvoice == null)
            {
                return;
            }

            PaymentInvoiceDto paymentInvoiceDto;

            switch (ownerType)
            {
                case Owner.Type.Request:
                    if (!paymentInvoice.RequestId.HasValue)
                        throw new ArgumentNullException(nameof(paymentInvoice.RequestId));

                    var request = _executor.GetQuery<GetRequestByIdQuery>()
                        .Process(q => q.ExecuteAsync(paymentInvoice.RequestId.Value)).Result;
                    paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                        opt => opt.Items["RequestCustomers"] = request.RequestCustomers);
                    break;
                case Owner.Type.Contract:
                    if (!paymentInvoice.ContractId.HasValue)
                        throw new ArgumentNullException(nameof(paymentInvoice.ContractId));

                    var contract = _executor.GetQuery<GetContractByIdQuery>()
                        .Process(q => q.ExecuteAsync(paymentInvoice.ContractId.Value)).Result;
                    paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                        opt => opt.Items["ContractCustomers"] = contract.ContractCustomers);
                    break;
                case Owner.Type.ProtectionDoc:
                    if (!paymentInvoice.ProtectionDocId.HasValue)
                        throw new ArgumentNullException(nameof(paymentInvoice.ProtectionDocId));

                    var protectionDoc = _executor.GetQuery<GetProtectionDocByIdQuery>()
                        .Process(q => q.ExecuteAsync(paymentInvoice.ProtectionDocId.Value)).Result;
                    paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                        opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers);
                    break;
                default:
                    throw new ApplicationException(string.Empty,
                        new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
            }

            if (decimal.Round(paymentInvoiceDto.Remainder, 2) < decimal.Round(paymentUse.Amount, 2))
            {
                throw new OptimisticConcurrencyException("Using amount is bigger than remainder!");
            }

            var paymentRepository = Uow.GetRepository<Payment>();
            var payment = paymentRepository
                .AsQueryable()
                .Include(p => p.PaymentUses)
                .First(p => p.Id == paymentUse.PaymentId.Value);

            var paymentDto = _mapper.Map<Payment, PaymentDto>(payment);

            if (decimal.Round(paymentDto.RemainderAmount, 2) < decimal.Round(paymentUse.Amount, 2))
            {
                throw new OptimisticConcurrencyException("Using amount is bigger than payment remainder!");
            }

            if (decimal.Round(paymentInvoiceDto.Remainder, 2) == decimal.Round(paymentUse.Amount, 2) || force && IsPaymentInForeignCurrency(paymentUse))
            {
                paymentInvoice.Status = Uow.GetRepository<DicPaymentStatus>().AsQueryable()
                    .FirstOrDefault(q => q.Code == DicPaymentStatusCodes.Credited);
                paymentInvoice.DateFact = DateTimeOffset.Now;
                paymentInvoice.WhoBoundUserId = NiisAmbientContext.Current.User.Identity.UserId;

                var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
                paymentInvoiceRepository.Update(paymentInvoice);
                Uow.SaveChanges();
            }
        }

        private bool IsPaymentInForeignCurrency(PaymentUse paymentUse)
        {
            if (paymentUse.PaymentId == null)
            {
                return false;
            }

            var payment = _executor.GetQuery<GetPaymentByIdQuery>()
                .Process(q => q.ExecuteAsync(paymentUse.PaymentId.Value)).Result;

            return payment != null && payment.IsForeignCurrency;
        }
    }
}