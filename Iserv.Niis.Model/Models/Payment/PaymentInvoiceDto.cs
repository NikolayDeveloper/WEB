using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Payment
{
    public class PaymentInvoiceDto
    {
        public int Id { get; set; }

        public int? OwnerId { get; set; }

        public int TariffId { get; set; }

        /// <summary> Наименование услуги </summary>
        [Display(Name = "Наименование услуги")]
        public string TariffNameRu { get; set; }

        /// <summary> Коэффициент </summary>
        [Display(Name = "Коэффициент")]
        public decimal Coefficient { get; set; }

        /// <summary> Количество </summary>
        [Display(Name = "Количество")]
        public int? TariffCount { get; set; }

        /// <summary> Штраф </summary>
        [Display(Name = "Штраф")]
        public decimal PenaltyPercent { get; set; }

        /// <summary> НДС </summary>
        [Display(Name = "НДС")]
        public decimal Nds { get; set; }

        /// <summary> Тариф </summary>
        [Display(Name = "Тариф")]
        public decimal? TariffPrice { get; set; }

        /// <summary> Сумма выплаты </summary>
        [Display(Name = "Сумма выплаты")]
        public decimal TotalAmount { set; get; }

        /// <summary> Сумма с НДС </summary>
        [Display(Name = "Сумма с НДС")]
        public decimal TotalAmountNds { get; set; }

        /// <summary> Фактическая сумма </summary>
        [Display(Name = "Фактическая сумма")]
        public decimal AmountUseSum { set; get; }

        /// <summary> Непогашенная сумма </summary>
        [Display(Name = "Непогашенная сумма")]
        public decimal Remainder { get; set; }

        public int StatusId { get; set; }

        public string StatusCode { get; set; }

        /// <summary> Статус оплаты </summary>
        [Display(Name = "Статус оплаты")]
        public string StatusNameRu { get; set; }

        /// <summary> Код услуги </summary>
        [Display(Name = "Код услуги")]
        public string TariffCode { get; set; }

        /// <summary> Дата выставления счёта на оплату услуги </summary>
        [Display(Name = "Дата выставления счёта на оплату услуги")]
        public DateTimeOffset? CreateDate { get; set; }

        /// <summary> Исполнитель (кто добавил счёт) </summary>
        [Display(Name = "Исполнитель (кто добавил счёт)")]
        public string CreateUser { get; set; }

        /// <summary> Дата зачтения оплаты </summary>
        [Display(Name = "Дата зачтения оплаты")]
        public DateTimeOffset? CreditDate { get; set; }

        /// <summary> Исполнитель (кто зачёл) </summary>
        [Display(Name = "Исполнитель (кто зачёл)")]
        public string CreditUser { get; set; }

        /// <summary> Дата списания оплаты </summary>
        [Display(Name = "Дата списания оплаты")]
        public DateTimeOffset? WriteOffDate { get; set; }

        /// <summary> Исполнитель (кто списал) </summary>
        [Display(Name = "Исполнитель (кто списал)")]
        public string WriteOffUser { get; set; }

        /// <summary> Дата удаления выставленной оплаты </summary>
        [Display(Name = "Дата удаления выставленной оплаты")]
        public DateTimeOffset? DeleteDate { get; set; }

        /// <summary> Исполнитель (кто удалил выставленную оплату) </summary>
        [Display(Name = "Исполнитель (кто удалил выставленную оплату)")]
        public string DeleteUser { get; set; }

        /// <summary> Причина удаления выставленной оплаты </summary>
        [Display(Name = "Причина удаления выставленной оплаты")]
        public string DeletionReason { get; set; }

        public bool IsDeleted { get; set; }
    }
}