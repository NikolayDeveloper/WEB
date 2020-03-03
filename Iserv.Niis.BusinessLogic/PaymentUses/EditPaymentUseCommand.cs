using AutoMapper;
using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class EditPaymentUseCommand : BaseCommand
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public EditPaymentUseCommand(IMapper mapper, IExecutor executor)
        {
            _mapper = mapper;
            _executor = executor;
        }

        public async Task<EditPaymentUseResponseDto> ExecuteAsync(int paymentUseId, EditPaymentUseRequestDto requestDto)
        {
            var responseDto = new EditPaymentUseResponseDto();

            var paymentUseRepository = Uow.GetRepository<PaymentUse>();
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            var paymentRepository = Uow.GetRepository<Payment>();

            var paymentUse = await paymentUseRepository.AsQueryable()
                .Include(u => u.PaymentInvoice)
                .FirstOrDefaultAsync(u => u.Id == paymentUseId);

            if (paymentUse.PaymentInvoice.DateComplete != null)
            {
                throw new ValidationException("Cannot edit Charged PaymentUse.");
            }

            var paymentInvoice = await _executor.GetQuery<GetPaymentInvoiceByIdQuery>()
                .Process(q => q.ExecuteAsync(paymentUse.PaymentInvoiceId.Value));

            PaymentInvoiceDto paymentInvoiceDto;

            if (paymentInvoice.RequestId != null)
            {
                var request = await _executor.GetQuery<GetRequestByIdQuery>()
                    .Process(q => q.ExecuteAsync(paymentInvoice.RequestId.Value));

                paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                    opt => opt.Items["RequestCustomers"] = request.RequestCustomers);
            }
            else if (paymentInvoice.ContractId != null)
            {
                var contract = await _executor.GetQuery<GetContractByIdQuery>()
                    .Process(q => q.ExecuteAsync(paymentInvoice.ContractId.Value));

                paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                    opt => opt.Items["ContractCustomers"] = contract.ContractCustomers);
            }
            else if (paymentInvoice.ProtectionDocId != null)
            {
                var protectionDoc = await _executor.GetQuery<GetProtectionDocByIdQuery>()
                    .Process(q => q.ExecuteAsync(paymentInvoice.ProtectionDocId.Value));

                paymentInvoiceDto = _mapper.Map<PaymentInvoice, PaymentInvoiceDto>(paymentInvoice,
                    opt => opt.Items["ProtectionDocCustomers"] = protectionDoc.ProtectionDocCustomers);
            }
            else
            {
                throw new ApplicationException("PaymentInvoice does not have owner record.");
            }

            var invoiceReminder = paymentInvoiceDto.Remainder + paymentUse.Amount;
            var invoiceReminderRnd = decimal.Round(invoiceReminder, 2);

            var paymentUseNewAmountRnd = decimal.Round(requestDto.Amount, 2);

            responseDto.PaymentInvoiceReminder = invoiceReminderRnd;
            if (invoiceReminderRnd < paymentUseNewAmountRnd)
            {
                responseDto.AmountIsGreaterThanPaymentInvoiceReminder = true;
            }

            var payment = await paymentRepository
                .AsQueryable()
                .Include(p => p.PaymentUses)
                .FirstOrDefaultAsync(p => p.Id == paymentUse.PaymentId.Value);

            var paymentDto = _mapper.Map<Payment, PaymentDto>(payment);

            var paymentReminder = paymentDto.RemainderAmount + paymentUse.Amount;
            var paymentReminderRnd = decimal.Round(paymentReminder, 2);

            responseDto.PaymentReminder = paymentReminderRnd;
            if (paymentReminderRnd < paymentUseNewAmountRnd)
            {
                responseDto.AmountIsGreaterThanPaymentReminder = true;
            }

            paymentInvoice = paymentUse.PaymentInvoice;

            if (!responseDto.AmountIsGreaterThanPaymentInvoiceReminder && !responseDto.AmountIsGreaterThanPaymentReminder)
            {
                if (requestDto.MakeCredited)
                {                   
                    if (invoiceReminderRnd - paymentUseNewAmountRnd > 100.0m)
                    {
                        responseDto.PaymentInvoiceNewReminderIsGreaterThan100KZT = true;
                    }
                    else
                    {
                        paymentUse.Amount = paymentUseNewAmountRnd;
                        FillPaymentUseEditInfo(paymentUse, requestDto);
                        paymentUseRepository.Update(paymentUse);

                        MakePaymentInvoiceCredited(paymentInvoice);
                        paymentInvoiceRepository.Update(paymentInvoice);

                        Uow.SaveChanges();

                        responseDto.Success = true;
                    }
                }
                else
                {
                    paymentUse.Amount = paymentUseNewAmountRnd;
                    FillPaymentUseEditInfo(paymentUse, requestDto);
                    paymentUseRepository.Update(paymentUse);

                    if (paymentUseNewAmountRnd == invoiceReminderRnd)
                    {
                        MakePaymentInvoiceCredited(paymentInvoice);
                        paymentInvoiceRepository.Update(paymentInvoice);
                    }
                    else if (paymentUseNewAmountRnd < invoiceReminderRnd)
                    {
                        MakePaymentInvoiceNotpaid(paymentInvoice);
                        paymentInvoiceRepository.Update(paymentInvoice);
                    }

                    Uow.SaveChanges();

                    responseDto.Success = true;
                }
            }

            if (responseDto.Success)
            {
                _executor.GetCommand<UpdatePaymentStatusCommand>()
                    .Process(c => c.Execute(paymentUse.PaymentId.Value));
            }

            return responseDto;
        }

        private void MakePaymentInvoiceCredited(PaymentInvoice paymentInvoice)
        {
            paymentInvoice.Status = Uow.GetRepository<DicPaymentStatus>()
                .AsQueryable()
                .FirstOrDefault(q => q.Code == DicPaymentStatusCodes.Credited);

            paymentInvoice.DateFact = DateTimeOffset.Now;
            paymentInvoice.WhoBoundUserId = NiisAmbientContext.Current.User.Identity.UserId;
        }

        private void MakePaymentInvoiceNotpaid(PaymentInvoice paymentInvoice)
        {
            paymentInvoice.Status = Uow.GetRepository<DicPaymentStatus>()
                .AsQueryable()
                .FirstOrDefault(q => q.Code == DicPaymentStatusCodes.Notpaid);

            paymentInvoice.DateFact = null;
            paymentInvoice.WhoBoundUserId = null;
        }

        private void FillPaymentUseEditInfo(PaymentUse paymentUse, EditPaymentUseRequestDto requestDto)
        {
            var user = _executor.GetQuery<GetUserByIdQuery>()
                .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));

            paymentUse.EditClearedPaymentDate = DateTimeOffset.Now;
            paymentUse.EditClearedPaymentEmployeeName = $"{user.NameRu} {user.Position?.NameRu}";
            paymentUse.EditClearedPaymentReason = requestDto.EditReason;
        }
    }
}