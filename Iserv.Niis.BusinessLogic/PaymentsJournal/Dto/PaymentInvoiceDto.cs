using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Iserv.Niis.Domain.Entities.Payment;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class PaymentInvoiceDto
    {
        /// <summary> ID </summary>
        public int Id { get; set; }

        /// <summary> Код услуги </summary>
        public string ServiceCode { get; set; }

        /// <summary> Наименование услуги </summary>
        public string ServiceName { get; set; }

        /// <summary> Тариф </summary>
        public decimal Price { get; set; }

        /// <summary> Коэффициент </summary>
        public decimal Coefficient { get; set; }

        /// <summary> Количество </summary>
        public int Count { get; set; }

        /// <summary> Штраф </summary>
        public decimal Penalty { get; set; }

        /// <summary> НДС </summary>
        public decimal Nds { get; set; }

        /// <summary> Сумма выплаты </summary>
        public decimal Amount => Price * Count * Coefficient * (1 + Penalty);

        /// <summary> Сумма с НДС </summary>
        public decimal AmountNds => Amount * (1 + Nds);

        /// <summary> Фактическая сумма </summary>
        public decimal PayedAmount { get; set; }

        /// <summary> Код статуса оплаты </summary>
        public string StatusCode { get; set; }

        /// <summary> Название статуса оплаты </summary>
        public string StatusName { get; set; }

        /// <summary> Не погашенная сумма </summary>
        public decimal RemainsAmount => AmountNds - PayedAmount;

        /// <summary> Дата выставления счёта на оплату услуги </summary>
        public DateTimeOffset DateCreate { get; set; }

        /// <summary> Исполнитель (кто добавил счёт) </summary>
        public string CreateUserName { get; set; }

        /// <summary> Дата зачтения оплаты </summary>
        public DateTimeOffset? DateBound { get; set; }

        /// <summary> Исполнитель (кто зачёл) </summary>
        public string BoundUserName { get; set; }

        /// <summary> Дата списания </summary>
        public DateTimeOffset? DateComplete { get; set; }

        /// <summary> Исполнитель (кто списал) </summary>
        public string CompleteUserName { get; set; }

        /// <summary> Дата удаления выставленной оплаты </summary>
        public DateTimeOffset? DateDelete { get; set; }

        /// <summary> Исполнитель (кто удалил выставленную оплату) </summary>
        public string DeleteUserName { get; set; }

        /// <summary> Причина удаления выставленной оплаты </summary>
        public string DeleteReason { get; set; }

        [IgnoreDataMember]
        public static Expression<Func<PaymentInvoice, PaymentInvoiceDto>> FromPaymentInvoice = pi => new PaymentInvoiceDto
        {
            Id = pi.Id,
            ServiceCode = pi.Tariff != null ? pi.Tariff.Code : null,
            ServiceName = pi.Tariff != null ? pi.Tariff.NameRu : null,
            Price = pi.Tariff != null && pi.Tariff.Price != null ? pi.Tariff.Price.Value : 0,
            Coefficient = pi.Coefficient,
            Count = pi.TariffCount != null ? pi.TariffCount.Value : 0,
            Penalty = pi.PenaltyPercent,
            Nds = pi.Nds,
            PayedAmount = pi.PaymentUses.Sum(x => x.Amount),
            StatusCode = pi.Status != null ? pi.Status.Code : null,
            StatusName = pi.Status != null ? pi.Status.NameRu : null,
            DateCreate = pi.DateCreate,
            CreateUserName = pi.CreateUser != null ? pi.CreateUser.NameRu : null,
            DateBound = pi.DateFact,
            BoundUserName = pi.WhoBoundUser != null ? pi.WhoBoundUser.NameRu : null,
            DateComplete = pi.DateComplete,
            CompleteUserName = pi.WriteOffUser != null ? pi.WriteOffUser.NameRu : null,
            DateDelete = null,
            DeleteUserName = null,
            DeleteReason = null
        };
    }
}