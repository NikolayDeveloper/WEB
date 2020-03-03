using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Iserv.Niis.Domain.Entities.Payment;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class PaymentUseDto
    {
        /// <summary> ID </summary>
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary> Наименование плательщика </summary>
        [Display(Name = "Наименование плательщика")]
        public string PayerName { get; set; }

        /// <summary> ИИН\БИН плательщика </summary>
        [Display(Name = @"ИИН\БИН плательщика")]
        public string PayerXin { get; set; }

        /// <summary> РНН плательщика </summary>
        [Display(Name = "РНН плательщика")]
        public string PayerRnn { get; set; }

        /// <summary> Код услуги </summary>
        [Display(Name = "Код услуги")]
        public string ServiceCode { get; set; }

        /// <summary> Наименование услуги </summary>
        [Display(Name = "Наименование услуги")]
        public string ServiceName { get; set; }

        /// <summary> Номер ОД </summary>
        [Display(Name = "Номер ОД")]
        public string ProtectionDocNumber { get; set; }

        /// <summary> Номер заявки </summary>
        [Display(Name = "Номер заявки")]
        public string RequestNumber { get; set; }

        /// <summary> Вид ОПС </summary>
        [Display(Name = "Вид ОПС")]
        public string ProtectionDocTypeName { get; set; }

        /// <summary> Наименование ОПС </summary>
        [Display(Name = "Наименование ОПС")]
        public string ProtectionDocName { get; set; }

        /// <summary> Номер Договора </summary>
        [Display(Name = "Номер Договора")]
        public string ContractNumber { get; set; }

        /// <summary> Вид заявления на регистрацию договора </summary>
        [Display(Name = "Вид заявления на регистрацию договора")]
        public string ContractTypeName { get; set; }

        /// <summary> Сумма </summary>
        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        /// <summary> Дата зачтения оплаты </summary>
        [Display(Name = "Дата зачтения оплаты")]
        public DateTimeOffset? PaymentUseDate { get; set; }

        /// <summary> Описание </summary>    
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary> Дата выставления оплаты услуги </summary>
        [Display(Name = "Дата выставления оплаты услуги")]
        public DateTimeOffset? IssuingPaymentDate { get; set; }

        /// <summary> Пользователь (ЗО) </summary>
        [Display(Name = "Пользователь (ЗО)")]
        public string EmployeeCheckoutPaymentName { get; set; }

        /// <summary> Дата списания оплаты </summary>
        [Display(Name = "Дата списания оплаты")]
        public DateTimeOffset? DateOfPayment { get; set; }

        /// <summary> Пользователь (СО) </summary>
        [Display(Name = "Пользователь (СО)")]
        public string EmployeeWriteOffPaymentName { get; set; }

        /// <summary> Дата и время удаления зачтённой оплаты </summary>
        [Display(Name = "Дата и время удаления зачтённой оплаты")]
        public DateTimeOffset? DeletionClearedPaymentDate { get; set; }

        /// <summary> Сотрудник, выполнивший удаление зачтённой оплаты </summary>
        [Display(Name = "Сотрудник, выполнивший удаление зачтённой оплаты")]
        public string DeletionClearedPaymentEmployeeName { get; set; }

        /// <summary> Причина удаления зачтённой оплаты </summary>
        [Display(Name = "Причина удаления зачтённой оплаты")]
        public string DeletionClearedPaymentReason { get; set; }

        /// <summary> Удален </summary>
        [Display(Name = "Удален")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Сотрудник, выполнивший редактирование зачтённой оплаты (ФИО и должность сотрудника, выполнившего редактирование зачтённой оплаты (см. UC.Plat.06))
        /// </summary>
        [Display(Name = "Сотрудник, выполнивший редактирование зачтённой оплаты")]
        public string EditClearedPaymentEmployeeName { get; set; }

        /// <summary>
        /// Дата и время редактирования зачтённой оплаты (Дата и время редактирования зачтённой оплаты (см. UC.Plat.06))
        /// </summary>
        [Display(Name = "Дата и время редактирования зачтённой оплаты")]
        public DateTimeOffset? EditClearedPaymentDate { get; set; }

        /// <summary>
        /// Причина редактирования зачтённой оплаты (Причина редактирования зачтённой оплаты (см. UC.Plat.06))
        /// </summary>
        [Display(Name = "Причина редактирования зачтённой оплаты")]
        public string EditClearedPaymentReason { get; set; }

        [IgnoreDataMember]
        public static Expression<Func<PaymentUse, PaymentUseDto>> FromPaymentUse = pu => new PaymentUseDto
        {
            Id = pu.Id,
            PayerName = pu.Payment != null && pu.Payment.Customer != null ? pu.Payment.Customer.NameRu : null,
            PayerXin = pu.Payment != null && pu.Payment.Customer != null ? pu.Payment.Customer.Xin : null,
            PayerRnn = pu.Payment != null && pu.Payment.Customer != null ? pu.Payment.Customer.Rnn : null,
            ServiceCode = pu.PaymentInvoice != null && pu.PaymentInvoice.Tariff != null ? pu.PaymentInvoice.Tariff.Code : null,
            ServiceName = pu.PaymentInvoice != null && pu.PaymentInvoice.Tariff != null ? pu.PaymentInvoice.Tariff.NameRu : null,
            ProtectionDocNumber = pu.PaymentInvoice.ProtectionDoc != null ? pu.PaymentInvoice.ProtectionDoc.GosNumber : null,
            RequestNumber = pu.Request != null ? pu.Request.RequestNum : null,
            ProtectionDocName = pu.PaymentInvoice.Request != null
                ? pu.PaymentInvoice.Request.NameRu
                : pu.PaymentInvoice.ProtectionDoc != null
                    ? pu.PaymentInvoice.ProtectionDoc.NameRu
                    : pu.PaymentInvoice.Contract != null
                        ? pu.PaymentInvoice.Contract.NameRu
                        : null,
            ProtectionDocTypeName =
                pu.PaymentInvoice.Request != null
                    ? pu.PaymentInvoice.Request.ProtectionDocType != null ? pu.PaymentInvoice.Request.ProtectionDocType.NameRu : null
                    : pu.PaymentInvoice.ProtectionDoc != null
                        ? pu.PaymentInvoice.ProtectionDoc.Type != null ? pu.PaymentInvoice.ProtectionDoc.Type.NameRu : null
                        : pu.PaymentInvoice.Contract != null
                            ? pu.PaymentInvoice.Contract.ProtectionDocType != null ? pu.PaymentInvoice.Contract.ProtectionDocType.NameRu : null
                            : null,
            ContractNumber = pu.PaymentInvoice.Contract != null ? pu.PaymentInvoice.Contract.GosNumber : null,
            ContractTypeName = pu.PaymentInvoice.Contract != null && pu.PaymentInvoice.Contract.Type != null ? pu.PaymentInvoice.Contract.Type.NameRu : null,
            Amount = pu.Amount,
            PaymentUseDate = pu.DateCreate,
            Description = pu.Description,
            IssuingPaymentDate = pu.IssuingPaymentDate != null ? pu.IssuingPaymentDate.Value.LocalDateTime : (DateTimeOffset?)null,
            EmployeeCheckoutPaymentName = pu.EmployeeCheckoutPaymentName,
            DateOfPayment = pu.DateOfPayment != null ? pu.DateOfPayment.Value.LocalDateTime : (DateTimeOffset?)null,
            EmployeeWriteOffPaymentName = pu.EmployeeWriteOffPaymentName,
            DeletionClearedPaymentDate = pu.DeletionClearedPaymentDate != null ? pu.DeletionClearedPaymentDate.Value.LocalDateTime : (DateTimeOffset?)null,
            DeletionClearedPaymentEmployeeName = pu.DeletionClearedPaymentEmployeeName,
            DeletionClearedPaymentReason = pu.DeletionClearedPaymentReason,
            IsDeleted = pu.IsDeleted,
            EditClearedPaymentDate = pu.EditClearedPaymentDate,
            EditClearedPaymentEmployeeName = pu.EditClearedPaymentEmployeeName,
            EditClearedPaymentReason = pu.EditClearedPaymentReason
        };
    }
}
