using Iserv.Niis.Domain.Entities.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Iserv.Niis.BusinessLogic.ModulePayment.Dtos
{
    public class PaymentJournalDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Контрагент (Наименование контрагента. Значение из 1С)
        /// </summary>
        public string Payer { get; set; }
        /// <summary>
        /// Сумма платежа (Сумма платежа.Значение из 1С)
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Остаток (Разница между Суммой платежа и зачтённой суммой)
        /// </summary>
        public decimal? CashBalance { get; set; }
        /// <summary>
        /// Распределено (Сумма, которая была зачтена за услугу(-ги)))
        /// </summary>
        public decimal? PaymentUseAmmountSumm { get; set; }

        /// <summary>
        ///  Номер платежа (Значение из 1С)
        /// </summary>
        public string Payment1CNumber { get; set; }
        /// <summary>
        /// Назначение (Краткое описание назначения платежа. Значение из 1С)
        /// </summary>
        public string PurposeDescription { get; set; }
        /// <summary>
        /// ИИН\БИН контрагента (ИИН\БИН контрагента совершившего платёж)
        /// </summary>
        public string PayerBinOrInn { get; set; }
        /// <summary>
        /// Дата платежа (Дата поступления платежа. Значение из 1С.)
        /// </summary>
        public DateTimeOffset? PaymentDate { get; set; }
        /// <summary>
        /// Дата создания записи (Дата сохранения платежа в АИС НИИС)
        /// </summary>
        public DateTimeOffset DateCreate { get; set; }
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
        public List<PaymentUsesDto> PaymentUses { get; set; }
        
        public int? PaymentStatusId { get; set; }
        public string PaymentStatusNameRu { get; set; }
        public string PaymentStatusNameKz { get; set; }
        public bool IsForeignCurrency { get; set; } 
        public string PaymentCNumberBVU { get; set; }
        /// <summary>
        /// Авансовый
        /// </summary>
        public bool IsAdvancePayment { get; set; }
        /// <summary>
        /// Сотрудник выполнивший возврат платежа
        /// </summary>
        public string EmployeeNameReturnedPayment { get; set; }
        /// <summary>
        /// Дата возврата
        /// </summary>
        public DateTimeOffset? ReturnedDate { get; set; }

        [IgnoreDataMember]
        public static Func<Payment, PaymentJournalDto> ToPaymentJournalDto = payment =>
        {
            return new PaymentJournalDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                CurrencyRate = payment.CurrencyRate,
                CurrencyType = payment.CurrencyType,
                DateCreate = payment.DateCreate,
                Payment1CNumber = payment.Payment1CNumber,
                PaymentDate = payment.PaymentDate,
                PaymentUses = payment.PaymentUses.Select(PaymentUsesDto.ToPaymentUsesDto).ToList(),
                PurposeDescription = payment.PurposeDescription,
                Payer = payment.Payer,
                PayerBinOrInn = payment.PayerBinOrInn,
                PaymentStatusId = payment.PaymentStatus?.Id,
                PaymentStatusNameRu = payment.PaymentStatus?.NameRu,
                PaymentStatusNameKz = payment.PaymentStatus?.NameKz,
                CashBalance = payment.CashBalance,
                IsForeignCurrency = payment.IsForeignCurrency,
                PaymentUseAmmountSumm = payment.PaymentUseAmmountSumm,
                EmployeeNameReturnedPayment = payment.EmployeeNameReturnedPayment,
                IsAdvancePayment = payment.IsAdvancePayment,
                PaymentCNumberBVU = payment.PaymentCNumberBVU,
                ReturnedDate = payment.ReturnedDate
            };
        };
    }
}