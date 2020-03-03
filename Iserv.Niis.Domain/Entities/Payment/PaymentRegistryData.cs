using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Payment
{
    public class PaymentRegistryData : Entity<int>, IHaveConcurrencyToken
    {
        public int? DocumentId { get; set; }
        public Document.Document Document { get; set; }
        public int? PaymentInvoiceId { get; set; }
        public PaymentInvoice PaymentInvoice { get; set; }
    }
}