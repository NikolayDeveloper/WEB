using System;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal.Dto
{
    public class PaymentsSearchParametersDto
    {
        private DateTime? dateCreateFrom;
        private DateTime? dateCreateTo;
        private DateTime? paymentDateFrom;
        private DateTime? paymentDateTo;

        /// <summary> ID </summary>
        public int? Id { get; set; }

        /// <summary> Дата создания записи с </summary>
        public DateTime? DateCreateFrom
        {
            get => this.dateCreateFrom;
            set => this.dateCreateFrom = value;
        }

        /// <summary> Дата создания записи по </summary>
        public DateTime? DateCreateTo
        {
            get => this.dateCreateTo;
            set => this.dateCreateTo = value;
        }

        /// <summary> ФИО/Наименование плательщика </summary>
        public string PayerName{ get; set; }

        /// <summary> ИИН/БИН плательщика </summary>
        public string PayerXin{ get; set; }

        /// <summary> РНН плательщика </summary>
        public string PayerRnn{ get; set; }

        /// <summary> Дата платежа с </summary>
        public DateTime? PaymentDateFrom
        {
            get => this.paymentDateFrom;
            set => this.paymentDateFrom = value;
        }

        /// <summary> Дата платежа по </summary>
        public DateTime? PaymentDateTo
        {
            get => this.paymentDateTo;
            set => this.paymentDateTo = value;
        }

        /// <summary> Сумма </summary>
        public decimal? Amount { get; set; }

        /// <summary> Остаток </summary>
        public decimal? Remainder { get; set; }

        /// <summary> Распределено </summary>
        public decimal? Distributed { get; set; }

        /// <summary> Назначение платежа </summary>
        public string PaymentPurpose { get; set; }

        /// <summary> Номер платежа </summary>
        public string PaymentNumber { get; set; }

        /// <summary> Номер документа 1С  </summary>
        public string PaymentDocumentNumber { get; set; }

        /// <summary> Статус платежа </summary>
        public int? PaymentStatusId { get; set; }

        /// <summary> Платёж в иностранной валюте </summary>
        public bool IsForeignCurrency { get; set; }

        /// <summary> Код валюты </summary>
        public string CurrencyCode { get; set; }
    }
}