using System;

namespace Iserv.Niis.Integration.OneC.Model
{
    /// <summary>
    /// Модель платежа получаемого из 1C.
    /// </summary>
    public class PaymentFrom1CDto
    {
        /// <summary>
        /// Номер платежа (Значение из 1С)
        /// </summary>
        public string Payment1CNumber { get; set; }

        /// <summary>
        /// Назначение (Краткое описание назначения платежа. Значение из 1С)
        /// </summary>
        public string PurposeDescription { get; set; }

        /// <summary>
        /// Плательщик (Наименование контрагента. Значение из 1С)
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// Сумма платежа (Сумма платежа.Значение из 1С)
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Номер документа 1С (Значение из 1С. Номер платежа, присвоенный БВУ. банковская выписка ??)
        /// </summary>
        public string PaymentCNumberBVU { get; set; }

        /// <summary>
        /// Дата платежа (Дата поступления платежа. Значение из 1С.)
        /// </summary>
        public DateTimeOffset? PaymentDate { get; set; }

        /// <summary>
        /// ИИН\БИН плательщика (ИИН\БИН плательщика совершившего платёж)
        /// </summary>
        public string PayerBinOrInn { get; set; }

        /// <summary>
        /// РНН контрагента совершившего платёж
        /// </summary>
        public string PayerRNN { get; set; }

        /// <summary>
        /// Тип валюты
        /// </summary>
        public string CurrencyType { get; set; }

        /// <summary>
        /// Юр. лицо/Физ. лицо
        /// </summary>
        public string CustomerType { get; set; }

        /// <summary>
        /// Является ли платеж авансовым (код 3510).
        /// </summary>
        public bool IsAdvancePayment { get; set; }
    }
}
