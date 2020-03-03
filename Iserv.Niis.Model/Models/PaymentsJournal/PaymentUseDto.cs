using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Iserv.Niis.Domain.Entities.Payment;

namespace Iserv.Niis.Model.Models.PaymentsJournal
{
    public class PaymentUseDto
    {
        /// <summary>
        /// Идентификатор связанного платежа.
        /// </summary>
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор платежа, из которого была зачтена сумма.
        /// </summary>
        [Display(Name = "Идентификатор платежа, из которого была зачтена сумма.")]
        public int? PaymentId { get; set; }

        /// <summary>
        /// Наименование плательщика
        /// </summary>
        [Display(Name = "Наименование плательщика")]
        public string PayerName { get; set; }

        /// <summary>
        /// ИИН\БИН плательщика
        /// </summary>
        [Display(Name = @"ИИН\БИН плательщика")]
        public string PayerXin { get; set; }

        /// <summary>
        /// РНН плательщика
        /// </summary>
        [Display(Name = "РНН плательщика")]
        public string PayerRnn { get; set; }

        /// <summary>
        /// Код услуги
        /// </summary>
        [Display(Name = "Код услуги")]
        public string ServiceCode { get; set; }

        /// <summary>
        /// Наименование услуги
        /// </summary>
        [Display(Name = "Наименование услуги")]
        public string ServiceName { get; set; }

        /// <summary>
        /// Номер ОД
        /// </summary>
        [Display(Name = "Номер ОД")]
        public string ProtectionDocNumber { get; set; }

        /// <summary>
        /// Номер заявки
        /// </summary>
        [Display(Name = "Номер заявки")]
        public string RequestNumber { get; set; }

        /// <summary>
        /// Вид ОПС
        /// </summary>
        [Display(Name = "Вид ОПС")]
        public string ProtectionDocTypeName { get; set; }

        /// <summary>
        /// Наименование ОПС
        /// </summary>
        [Display(Name = "Наименование ОПС")]
        public string ProtectionDocName { get; set; }

        /// <summary>
        /// Номер Договора
        /// </summary>
        [Display(Name = "Номер Договора")]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Вид заявления на регистрацию договора
        /// </summary>
        [Display(Name = "Вид заявления на регистрацию договора")]
        public string ContractTypeName { get; set; }

        /// <summary>
        /// Сумма платежа.
        /// </summary>
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// Сумма зачтения.
        /// </summary>
        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Дата зачтения оплаты
        /// </summary>
        [Display(Name = "Дата зачтения оплаты")]
        public DateTimeOffset? PaymentUseDate { get; set; }

        /// <summary>
        /// Дата платежа. Значение из 1С.
        /// </summary>
        public DateTimeOffset? PaymentDate { get; set; }

        /// <summary>
        /// Номер документа 1С.
        /// </summary>
        public string Payment1CNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>    
        [Display(Name = "Описание")]
        public string Description { get; set; }

        /// <summary>
        /// Дата выставления оплаты услуги
        /// </summary>
        [Display(Name = "Дата выставления оплаты услуги")]
        public DateTimeOffset? IssuingPaymentDate { get; set; }

        /// <summary>
        /// Пользователь (ЗО)
        /// </summary>
        [Display(Name = "Пользователь (ЗО)")]
        public string EmployeeCheckoutPaymentName { get; set; }

        /// <summary>
        /// Дата списания оплаты
        /// </summary>
        [Display(Name = "Дата списания оплаты")]
        public DateTimeOffset? DateOfPayment { get; set; }

        /// <summary>
        /// Пользователь (СО)
        /// </summary>
        [Display(Name = "Пользователь (СО)")]
        public string EmployeeWriteOffPaymentName { get; set; }

        /// <summary>
        /// Дата и время удаления зачтённой оплаты
        /// </summary>
        [Display(Name = "Дата и время удаления зачтённой оплаты")]
        public DateTimeOffset? DeletionClearedPaymentDate { get; set; }

        /// <summary>
        /// Сотрудник, выполнивший удаление зачтённой оплаты
        /// </summary>
        [Display(Name = "Сотрудник, выполнивший удаление зачтённой оплаты")]
        public string DeletionClearedPaymentEmployeeName { get; set; }

        /// <summary>
        /// Причина удаления зачтённой оплаты
        /// </summary>
        [Display(Name = "Причина удаления зачтённой оплаты")]
        public string DeletionClearedPaymentReason { get; set; }

        /// <summary>
        /// Удален
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Авансовый платеж.
        /// </summary>
        public bool IsAdvancePayment { get; set; }

        /// <summary>
        /// Назначение платежа.
        /// </summary>
        public string PaymentPurpose { get; set; }


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
        public static Expression<Func<PaymentUse, PaymentUseDto>> FromPaymentUse = paymentUse =>
            new PaymentUseDto
            {
                Id = paymentUse.Id,
                PaymentId = paymentUse.PaymentId,
                PayerName = paymentUse.Payment != null && paymentUse.Payment.Customer != null ? paymentUse.Payment.Customer.NameRu : null,
                PayerXin = paymentUse.Payment != null && paymentUse.Payment.Customer != null ? paymentUse.Payment.Customer.Xin : null,
                PayerRnn = paymentUse.Payment != null && paymentUse.Payment.Customer != null ? paymentUse.Payment.Customer.Rnn : null,
                ServiceCode = paymentUse.PaymentInvoice != null && paymentUse.PaymentInvoice.Tariff != null
                    ? paymentUse.PaymentInvoice.Tariff.Code
                    : null,
                ServiceName = paymentUse.PaymentInvoice != null && paymentUse.PaymentInvoice.Tariff != null
                    ? paymentUse.PaymentInvoice.Tariff.NameRu
                    : null,
                ProtectionDocNumber = paymentUse.PaymentInvoice.ProtectionDoc != null
                    ? paymentUse.PaymentInvoice.ProtectionDoc.GosNumber
                    : null,
                RequestNumber = paymentUse.Request != null ? paymentUse.Request.RequestNum : null,
                ProtectionDocName = paymentUse.PaymentInvoice.Request != null
                    ? paymentUse.PaymentInvoice.Request.NameRu
                    : paymentUse.PaymentInvoice.ProtectionDoc != null
                        ? paymentUse.PaymentInvoice.ProtectionDoc.NameRu
                        : paymentUse.PaymentInvoice.Contract != null
                            ? paymentUse.PaymentInvoice.Contract.NameRu
                            : null,
                ProtectionDocTypeName =
                    paymentUse.PaymentInvoice.Request != null
                        ? paymentUse.PaymentInvoice.Request.ProtectionDocType != null
                            ?
                            paymentUse.PaymentInvoice.Request.ProtectionDocType.NameRu
                            : null
                        : paymentUse.PaymentInvoice.ProtectionDoc != null
                            ? paymentUse.PaymentInvoice.ProtectionDoc.Type != null
                                ?
                                paymentUse.PaymentInvoice.ProtectionDoc.Type.NameRu
                                : null
                            : paymentUse.PaymentInvoice.Contract != null
                                ? paymentUse.PaymentInvoice.Contract.ProtectionDocType != null
                                    ?
                                    paymentUse.PaymentInvoice.Contract.ProtectionDocType.NameRu
                                    : null
                                : null,
                ContractNumber = paymentUse.PaymentInvoice.Contract != null ? paymentUse.PaymentInvoice.Contract.GosNumber : null,
                ContractTypeName = paymentUse.PaymentInvoice.Contract != null && paymentUse.PaymentInvoice.Contract.Type != null
                    ? paymentUse.PaymentInvoice.Contract.Type.NameRu
                    : null,
                Amount = paymentUse.Amount,
                PaymentAmount = paymentUse.Payment != null ? paymentUse.Payment.Amount : null,
                Payment1CNumber =  paymentUse.Payment != null ? paymentUse.Payment.Payment1CNumber : null,
                PaymentUseDate = paymentUse.DateCreate,
                Description = paymentUse.Description,
                IssuingPaymentDate = paymentUse.IssuingPaymentDate != null
                    ? paymentUse.IssuingPaymentDate.Value.LocalDateTime
                    : (DateTimeOffset?) null,
                EmployeeCheckoutPaymentName = paymentUse.EmployeeCheckoutPaymentName,
                DateOfPayment =
                    paymentUse.DateOfPayment != null ? paymentUse.DateOfPayment.Value.LocalDateTime : (DateTimeOffset?) null,
                EmployeeWriteOffPaymentName = paymentUse.EmployeeWriteOffPaymentName,
                DeletionClearedPaymentDate = paymentUse.DeletionClearedPaymentDate != null
                    ? paymentUse.DeletionClearedPaymentDate.Value.LocalDateTime
                    : (DateTimeOffset?) null,
                PaymentDate = paymentUse.Payment != null ? paymentUse.Payment.PaymentDate : null,
                DeletionClearedPaymentEmployeeName = paymentUse.DeletionClearedPaymentEmployeeName,
                DeletionClearedPaymentReason = paymentUse.DeletionClearedPaymentReason,
                IsDeleted = paymentUse.IsDeleted,
                EditClearedPaymentDate = paymentUse.EditClearedPaymentDate,
                EditClearedPaymentEmployeeName = paymentUse.EditClearedPaymentEmployeeName,
                EditClearedPaymentReason = paymentUse.EditClearedPaymentReason,
                IsAdvancePayment =  paymentUse.Payment != null && paymentUse.Payment.IsAdvancePayment,
                PaymentPurpose =  paymentUse.Payment != null ? paymentUse.Payment.PurposeDescription : null
            };
    }
}