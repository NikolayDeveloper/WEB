using Iserv.Niis.Domain.Entities.Payment;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class PaymentDto
    {
        /// <summary> ИД </summary>
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary> Плательщик </summary>
        [Display(Name = "Плательщик")]
        public string PayerName { get; set; }

        /// <summary> ИИН\БИН плательщика </summary>
        [Display(Name = @"ИИН\БИН плательщика")]
        public string PayerXin { get; set; }

        /// <summary> РНН плательщика </summary>
        [Display(Name = "РНН плательщика")]
        public string PayerRnn { get; set; }

        /// <summary> Сумма </summary>
        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        /// <summary> Остаток </summary>
        [Display(Name = "Остаток")]
        public decimal Remainder => Amount - Distributed - (ReturnedAmount ?? 0) - (BlockedAmount ?? 0);

        /// <summary> Распределено </summary>
        [Display(Name = "Распределено")]
        public decimal Distributed { get; set; }

        /// <summary>Возвращено</summary>
        [Display(Name = "Возвращено")]
        public decimal? ReturnedAmount { get; set; }

        /// <summary>Заблокировано</summary>
        [Display(Name = "Заблокировано")]
        public decimal? BlockedAmount { get; set; }

        /// <summary> Назначение платежа </summary>
        [Display(Name = "Назначение")]
        public string PaymentPurpose { get; set; }

        /// <summary> Дата платежа </summary>
        [Display(Name = "Дата платежа")]
        public DateTimeOffset? PaymentDate { get; set; }

        /// <summary> Номер платежа </summary>
        [Display(Name = "Номер платежа")]
        public string PaymentNumber { get; set; }

        /// <summary> Номер документа 1С  </summary>
        [Display(Name = "Номер документа 1С")]
        public string PaymentDocumentNumber { get; set; }

        /// <summary> Авансовый </summary>
        [Display(Name = "Авансовый")]
        public bool IsAdvancePayment { get; set; }

        /// <summary> Статус платежа </summary>
        [Display(Name = "Статус платежа")]
        public string PaymentStatusName { get; set; }

        /// <summary> Сотрудник выполнивший возврат платежа </summary>
        [Display(Name = "Сотрудник выполнивший возврат платежа")]
        public string RefundedEmployeeName { get; set; }

        /// <summary> Сотрудник выполнивший блокирование платежа </summary>
        [Display(Name = "Сотрудник выполнивший блокирование платежа")]
        public string BlockedEmployeeName { get; set; }

        /// <summary> Дата возврата </summary>
        [Display(Name = "Дата возврата")]
        public DateTimeOffset? RefundDate { get; set; }

        /// <summary> Дата блокирования </summary>
        [Display(Name = "Дата блокирования")]
        public DateTimeOffset? BlockedDate { get; set; }

        /// <summary> Платёж в иностранной валюте </summary>
        [Display(Name = "Платёж в иностранной валюте")]
        public bool IsForeignCurrency { get; set; }

        /// <summary> Код валюты </summary>
        [Display(Name = "Код валюты")]
        public string CurrencyCode { get; set; }

        /// <summary> Дата создания записи </summary>
        [Display(Name = "Дата создания записи")]
        public DateTimeOffset DateCreate { get; set; }

        /// <summary>
        /// Причина блокирования
        /// </summary>
        [Display(Name = "Причина блокирования")]
        public string BlockedReason { get; set; }

        /// <summary>
        /// Причина возврата
        /// </summary>
        [Display(Name = "Причина возврата")]
        public string ReturnedReason { get; set; }

        [IgnoreDataMember]
        public static Expression<Func<Payment, PaymentDto>> FromPaymentDto = payment => new PaymentDto
        {
            Id = payment.Id,
            PayerName = payment.Customer != null ? (!string.IsNullOrEmpty(payment.Customer.NameRu) ? payment.Customer.NameRu : !string.IsNullOrEmpty(payment.Customer.NameKz) ? payment.Customer.NameKz : payment.Customer.NameEn) : null,
            PayerXin = payment.Customer != null ? payment.Customer.Xin : null,
            PayerRnn = payment.Customer != null ? payment.Customer.Rnn : null,
            Amount = payment.Amount != null ? payment.Amount.Value : 0,
            Distributed = payment.PaymentUses.Where(x => !x.IsDeleted).Sum(x => x.Amount),
            ReturnedAmount = payment.ReturnedAmount,
            BlockedAmount = payment.BlockedAmount,
            PaymentPurpose = payment.PurposeDescription,
            PaymentDate = payment.PaymentDate,
            PaymentNumber = payment.PaymentNumber,
            PaymentDocumentNumber = payment.Payment1CNumber,
            IsAdvancePayment = payment.IsAdvancePayment,
            PaymentStatusName = payment.PaymentStatus != null ? payment.PaymentStatus.NameRu : null,
            RefundedEmployeeName = payment.EmployeeNameReturnedPayment,
            BlockedEmployeeName = payment.EmployeeNameBlockedPayment,
            RefundDate = payment.ReturnedDate,
            BlockedDate = payment.BlockedDate,
            IsForeignCurrency = payment.IsForeignCurrency,
            CurrencyCode = payment.CurrencyType,
            DateCreate = payment.DateCreate,
            ReturnedReason = payment.ReturnedReason,
            BlockedReason = payment.BlockedReason
        };
    }
}