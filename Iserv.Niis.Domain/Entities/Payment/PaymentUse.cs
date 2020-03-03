using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.EntitiesHistory.Payment;

namespace Iserv.Niis.Domain.Entities.Payment
{
    /// <summary>
    /// Распределенаня оплата (WT_PL_PAYMENT_USE)
    /// </summary>
    
    public class PaymentUse : Entity<int>, IHistorySupport, IHaveConcurrencyToken, ISoftDeletable
    {
        /// <summary>
        /// Наименование плательщика	Отображается наименование контрагента произведшего платёж. Значение из 1С.
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public string Payer { get; set; }

        /// <summary>
        /// ИИН\БИН плательщика	Отображается ИИН\БИН контрагента произведшего платёж. Значение из 1С.
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public string PayerBinOrInn { get; set; }

        [Obsolete("Дичь! Убрать!")]
        public int? DicTariffId { get; set; }

        /// <summary>
        /// Код услуги	Отображается код услуги, за которую зачтена оплата (Должны использоваться те же кода, которые используются в старой Системе)	Из справочника тарифов и услуг
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public DicTariff DicTariff { get; set; }

        /// <summary>
        /// Номер ОД (Регистрационный номер охранного документа, по которому оплачена услуга)
        /// </summary>
        public int? ProtectionDocId { get; set; }

        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }

        /// <summary>
        /// Номер заявки (Регистрационный номер заявки, по которой оплачена услуга)
        /// </summary>
        public int? RequestId { get; set; }

        public Request.Request Request { get; set; }

        /// <summary>
        /// Вид ОПС Отображается наименование вида ОПС по которому оплачена услуга.
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public int? DicProtectionDocTypeId { get; set; }

        [Obsolete("Дичь! Убрать!")]
        public DicProtectionDocType DicProtectionDocType { get; set; }

        /// <summary>
        /// Номер Договора (Номер договора по которому оплачена услуга
        /// </summary>
        public int? ContractId { get; set; }

        public Contract.Contract Contract { get; set; }

        [Obsolete("Дичь! Убрать!")]
        public int? DicProtectionDocSubTypeId { get; set; }

        /// <summary>
        /// Вид заявления на регистрацию договора (Наименование вида заявления на регистрацию договора.)
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public DicProtectionDocSubType DicProtectionDocSubType { get; set; }

        /// <summary>
        /// Сумма (Отображается зачтённая сумма с НДС)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Дата зачтения оплаты (Отображается Дата связки платежа с услугой)
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public DateTimeOffset? PaymentUseDate { get; set; }

        /// <summary>
        /// Описание (Отображается описание, которое было заполнено при зачтении оплаты)
        /// </summary>    
        public string Description { get; set; }

        /// <summary>
        /// Дата выставления оплаты услуги (Отображается Дата создания оплаты по услуге)
        /// </summary>
        public DateTimeOffset? IssuingPaymentDate { get; set; }

        /// <summary>
        /// Пользователь (ЗО) (Сотрудник, выполнивший зачтение оплаты)
        /// </summary>
        public string EmployeeCheckoutPaymentName { get; set; }

        /// <summary>
        /// Дата списания оплаты (Дата списания заполняется вручную (UC.Plat.07))
        /// </summary>
        public DateTimeOffset? DateOfPayment { get; set; }

        /// <summary>
        /// Пользователь (СО) (Сотрудник, списавший оплату. Если оплата была списана автоматически, то указывать значение «Авто»)
        /// </summary>
        public string EmployeeWriteOffPaymentName { get; set; }

        /// <summary>
        /// Заблокированная сумма (Отображается сумма, которая была заблокирована в связи с положительной курсовой разницей (см. UC.Plat.09))
        /// </summary>
        public decimal BlockedAmount { get; set; }

        /// <summary>
        /// Причина блокировка суммы платежа (Причина блокировка суммы платежа, указанная при блокировки части суммы платежа (см. UC.Plat.09))
        /// </summary>
        public string BlockedAmountReason { get; set; }

        /// <summary>
        /// Сотрудник, выполнивший блокировку части суммы платежа (ФИО и должность сотрудника, выполнившего блокировку части суммы платежа(см. UC.Plat.09))
        /// </summary>
        public string BlockedAmountEmployeeName { get; set; }

        /// <summary>
        /// Возвращённая сумма (Отображается сумма, которая была частично возвращена (см. UC.Plat.08))
        /// </summary>        
        public decimal ReturnedAmount { get; set; }

        /// <summary>
        /// Причина возврата части суммы платежа (Причина возврата части суммы платежа (см. UC.Plat.08))
        /// </summary>
        public string ReturnAmountReason { get; set; }

        /// <summary>
        /// Дата и время возврата части суммы платежа (Дата и время возврата части суммы платежа (см. UC.Plat.08))
        /// </summary>
        public DateTimeOffset? ReturnedAmountDate { get; set; }

        /// <summary>
        /// Сотрудник, выполнивший возврат части суммы платежа(ФИО и должность сотрудника, выполнившего возврат части суммы платежа (см. UC.Plat.08))
        /// </summary>
        public string ReturnAmountEmployeeName { get; set; }

        /// <summary>
        /// Дата и время удаления зачтённой оплаты (Дата и время удаления зачтённой оплаты (см. UC.Plat.05))
        /// </summary>
        public DateTimeOffset? DeletionClearedPaymentDate { get; set; }

        /// <summary>
        /// Сотрудник, выполнивший удаление зачтённой оплаты (ФИО и должность сотрудника, выполнившего удаления зачтённой оплаты (см. UC.Plat.05))
        /// </summary>
        public string DeletionClearedPaymentEmployeeName { get; set; }

        /// <summary>
        /// Причина удаления зачтённой оплаты (Причина удаления зачтённой оплаты (см. UC.Plat.05))
        /// </summary>
        public string DeletionClearedPaymentReason { get; set; }

        /// <summary>
        /// Сотрудник, выполнивший редактирование зачтённой оплаты (ФИО и должность сотрудника, выполнившего редактирование зачтённой оплаты (см. UC.Plat.06))
        /// </summary>
        public string EditClearedPaymentEmployeeName { get; set; }

        /// <summary>
        /// Дата и время редактирования зачтённой оплаты (Дата и время редактирования зачтённой оплаты (см. UC.Plat.06))
        /// </summary>
        public DateTimeOffset? EditClearedPaymentDate { get; set; }

        /// <summary>
        /// Причина редактирования зачтённой оплаты (Причина редактирования зачтённой оплаты (см. UC.Plat.06))
        /// </summary>
        public string EditClearedPaymentReason { get; set; }

        public int? PaymentId { get; set; }

        public Payment Payment { get; set; }

        public int? PaymentInvoiceId { get; set; }
        public PaymentInvoice PaymentInvoice { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(PaymentUseHistory);
        }
    }
}