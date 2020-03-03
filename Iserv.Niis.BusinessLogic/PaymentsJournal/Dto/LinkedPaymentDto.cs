using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Iserv.Niis.Domain.Entities.Payment;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class LinkedPaymentDto
    {
        public string PayerName { get; set; }

        public decimal Amount { get; set; }

        public decimal Remainder => Amount - Distributed - (ReturnedAmount ?? 0) - (BlockedAmount ?? 0);

        public decimal Distributed { get; set; }

        public decimal? ReturnedAmount { get; set; }

        public decimal? BlockedAmount { get; set; }

        public decimal PaymentUseAmount { get; set; }

        public DateTimeOffset? PaymentUseDate { get; set; }

        public string PaymentUseEmployeeName { get; set; }

        public string PaymentStatusName { get; set; }

        public string PaymentPurpose { get; set; }

        public DateTimeOffset? PaymentDate { get; set; }

        public string PaymentNumber { get; set; }

        public string PayerXin { get; set; }

        public DateTimeOffset? PaymentDateCreate { get; set; }

        public int PaymentId { get; set; }

        public string PaymentDocumentNumber { get; set; }

        public bool IsAdvancePayment { get; set; }

        public bool IsForeignCurrency { get; set; }

        public string CurrencyCode { get; set; }

        public DateTimeOffset? DeletionClearedPaymentDate { get; set; }

        public string DeletionClearedPaymentEmployeeName { get; set; }

        public string DeletionClearedPaymentReason { get; set; }

        [IgnoreDataMember]
        public static Expression<Func<PaymentUse, LinkedPaymentDto>> FromPaymentUse = pu => new LinkedPaymentDto
        {
            PayerName = pu.Payment.Customer != null ? pu.Payment.Customer.NameRu : null,
            Amount = pu.Payment.Amount != null ? pu.Payment.Amount.Value : 0,
            Distributed = pu.Payment.PaymentUses.Where(x => !x.IsDeleted).Sum(x => x.Amount),
            ReturnedAmount = pu.Payment.ReturnedAmount,
            BlockedAmount = pu.Payment.BlockedAmount,
            PaymentUseAmount = pu.Amount,
            PaymentUseDate = pu.DateCreate,
            PaymentUseEmployeeName = pu.EmployeeCheckoutPaymentName,
            PaymentStatusName = pu.Payment.PaymentStatus != null ? pu.Payment.PaymentStatus.NameRu : null,
            PaymentPurpose = pu.Payment.PurposeDescription,
            PaymentDate = pu.Payment.PaymentDate,
            PaymentNumber = pu.Payment.PaymentNumber,
            PayerXin = pu.Payment.Customer != null ? pu.Payment.Customer.Xin : null,
            PaymentDateCreate = pu.Payment.DateCreate,
            PaymentId = pu.Payment.Id,
            PaymentDocumentNumber = pu.Payment.Payment1CNumber,
            IsAdvancePayment = pu.Payment.IsAdvancePayment,
            IsForeignCurrency = pu.Payment.IsForeignCurrency,
            CurrencyCode = pu.Payment.CurrencyType,
            DeletionClearedPaymentDate = pu.DeletionClearedPaymentDate != null ? pu.DeletionClearedPaymentDate.Value.LocalDateTime : (DateTimeOffset?)null,
            DeletionClearedPaymentEmployeeName = pu.DeletionClearedPaymentEmployeeName,
            DeletionClearedPaymentReason = pu.DeletionClearedPaymentReason
        };
    }
}