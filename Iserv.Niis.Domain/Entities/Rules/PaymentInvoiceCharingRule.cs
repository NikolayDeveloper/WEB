using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Rules
{
    /// <summary>
    /// Правило списания платежей.
    /// </summary>
    public class PaymentInvoiceChargingRule : Entity<int>
    {
        /// <summary>
        /// Идентификатор этапа маршрута.
        /// </summary>
        public int StageId { get; set; }
        /// <summary>
        /// Этап маршрута.
        /// </summary>
        public DicRouteStage Stage { get; set; }

        /// <summary>
        /// Идентификатор следующего этапа маршрута.
        /// </summary>
        public int NextStageId { get; set; }
        /// <summary>
        /// Следующий этап маршрута.
        /// </summary>
        public DicRouteStage NextStage { get; set; }

        /// <summary>
        /// Идентификатор тарифа.
        /// </summary>
        public int TariffId { get; set; }
        /// <summary>
        /// Тариф.
        /// </summary>
        public DicTariff Tariff { get; set; }
    }
}
