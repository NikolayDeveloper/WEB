using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.AutoPaymentInvoiceGeneration
{
    public class PaymentInvoiceGenerationRule : Entity<int>
    {
        public int TariffId { get; set; }
        public DicTariff Tariff { get; set; }
        public int? StageId { get; set; }
        public DicRouteStage Stage { get; set; }
    }
}
