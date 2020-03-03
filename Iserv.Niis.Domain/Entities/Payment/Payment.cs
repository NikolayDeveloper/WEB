using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.EntitiesHistory.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Domain.Entities.Payment
{
    /// <summary>
    /// Платежи (WT_PL_PAYMENT)
    /// </summary>
    public class Payment : Entity<int>, IHistorySupport, IHaveConcurrencyToken
    {
        public Payment()
        {
            PaymentUses = new HashSet<PaymentUse>();
        }
        public int? CustomerId { get; set; }
        public DicCustomer Customer { get; set; }
        
        public string PaymentNumber { get; set; }

        public bool? IsPrePayment { get; set; }
        public string AssignmentDescription { get; set; }
        public decimal? CurrencyAmount { get; set; }

        /// <summary>
        /// Плательщик (Наименование контрагента. Значение из 1С)
        /// </summary>
        public string Payer { get; set; }
        /// <summary>
        /// Сумма платежа (Сумма платежа.Значение из 1С)
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Остаток (Разница между Суммой платежа и зачтённой суммой)
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public decimal? CashBalance { get; set; }
        /// <summary>
        /// Распределено (Сумма, которая была зачтена за услугу(-ги)))
        /// </summary>
        [Obsolete("Дичь! Убрать!")]
        public decimal? PaymentUseAmmountSumm { get; set; }
        /// <summary>
        /// Назначение (Краткое описание назначения платежа. Значение из 1С)
        /// </summary>
        public string PurposeDescription { get; set; }
        /// <summary>
        /// Дата платежа (Дата поступления платежа. Значение из 1С.)
        /// </summary>
        public DateTimeOffset? PaymentDate { get; set; }
        /// <summary>
        /// Номер платежа (Значение из 1С)
        /// </summary>
        public string Payment1CNumber { get; set; }
        /// <summary>
        /// Номер документа 1С (Значение из 1С. Номер платежа, присвоенный БВУ. банковская выписка ??)
        /// </summary>
        public string PaymentCNumberBVU { get; set; }
        /// <summary>
        /// ИИН\БИН плательщика (ИИН\БИН плательщика совершившего платёж)
        /// </summary>
        [MaxLength(12)]
        public string PayerBinOrInn { get; set; }
        /// <summary>
        /// РНН контрагента совершившего платёж
        /// </summary>
        [MaxLength(12)]
        public string PayerRNN { get; set; }
        /// <summary>
        /// Авансовый
        /// </summary>
        public bool IsAdvancePayment { get; set; }
        /// <summary>
        /// Статус платежа
        /// </summary>
        public DicPaymentStatus PaymentStatus { get; set; }
        /// <summary>
        /// Сотрудник выполнивший возврат платежа
        /// </summary>
        public string EmployeeNameReturnedPayment { get; set; }
        /// <summary>
        /// Дата возврата
        /// </summary>
        public DateTimeOffset? ReturnedDate { get; set; }
        /// <summary>
        /// Сумма возврата
        /// </summary>
        public decimal? ReturnedAmount { get; set; }
        /// <summary>
        /// Сотрудник выполнивший блокирование платежа
        /// </summary>
        public string EmployeeNameBlockedPayment { get; set; }
        /// <summary>
        /// Причина возврата
        /// </summary>
        public string ReturnedReason { get; set; }
        /// <summary>
        /// Дата блокирования
        /// </summary>
        public DateTimeOffset? BlockedDate { get; set; }
        /// <summary>
        /// Блокированная сумма
        /// </summary>
        public decimal? BlockedAmount { get; set; }
        /// <summary>
        /// Причина блокирования
        /// </summary>
        public string BlockedReason { get; set; }
        /// <summary>
        /// Платёж в иностранной валюте
        /// </summary>
        public bool IsForeignCurrency { get; set; }
        /// <summary>
        /// Курс Валюты
        /// </summary>
        public decimal? CurrencyRate { get; set; }
        /// <summary>
        /// Тип валюты
        /// </summary>
        public string CurrencyType { get; set; }
        /// <summary>
        /// Распределенная оплата (таблица-связка счета и платежа)
        /// </summary>
        public ICollection<PaymentUse> PaymentUses { get; set; }
        public int? PaymentStatusId { get; set; }
        /// <summary>
        /// Дата и время создания импортированной из 1С записи
        /// </summary>
        public DateTimeOffset? ImportedDate { get; set; }
        /// <summary>
        /// Сотрудник инициировавший загрузку из 1С
        /// </summary>
        public ApplicationUser UserImported { get; set; }
        public int? UserImportedId { get; set; }
        /// <summary>
        /// Имя сотрудника инициировавшего загрузку из 1С
        /// </summary>
        public string UserNameImported { get; set; }
        /// <summary>
        /// Должность сотрудника инициировавшего загрузку из 1С
        /// </summary>
        public string UserPositionImported { get; set; }

        public override string ToString()
        {
            return string.Format("${0} - {1}", PurposeDescription, Amount);
        }

        public Type GetHistoryEntity()
        {
            return typeof(PaymentHistory);
        }
    }
}