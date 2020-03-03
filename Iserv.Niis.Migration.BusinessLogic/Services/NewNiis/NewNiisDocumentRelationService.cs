using System.Collections.Generic;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Payment;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisDocumentRelationService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisDocumentRelationService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangeDocumentEarlyRegs(List<DocumentEarlyReg> documentEarlyReg)
        {
            _context.DocumentEarlyRegs.AddRange(documentEarlyReg);
            _context.SaveChanges();
        }

        public void CreateRangeDocumentCustomers(List<DocumentCustomer> documentCustomers)
        {
            _context.DocumentCustomers.AddRange(documentCustomers);
            _context.SaveChanges();
        }

        public void CreateRangeDocumentExecutors(List<DocumentExecutor> documentExecutors)
        {
            _context.DocumentExecutors.AddRange(documentExecutors);
            _context.SaveChanges();
        }

        public void CreateDocumentUserSignature(DocumentUserSignature documentUserSignature)
        {
            _context.DocumentUserSignatures.Add(documentUserSignature);
            _context.SaveChanges();
        }

        public void CreateRangePaymentRegistryDatas(List<PaymentRegistryData> paymentRegistryDatas)
        {
            _context.PaymentRegistryDatas.AddRange(paymentRegistryDatas);
            _context.SaveChanges();
        }
    }
}
