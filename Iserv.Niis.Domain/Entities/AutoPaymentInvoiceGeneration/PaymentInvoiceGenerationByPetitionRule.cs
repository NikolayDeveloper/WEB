using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.AutoPaymentInvoiceGeneration
{
    public class PaymentInvoiceGenerationByPetitionRule: Entity<int>
    {
        public int TariffId { get; set; }
        public DicTariff Tariff { get; set; }
        public int PetitionTypeId { get; set; }
        public DicDocumentType PetitionType { get; set; }
    }
}
