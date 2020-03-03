using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisDocumentService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisDocumentService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangeDocuments(List<Document> documents)
        {
            _context.Documents.AddRange(documents);
            _context.SaveChanges();
        }

        public void UpdateRangeDocuments(List<Document> documents)
        {
            _context.Documents.UpdateRange(documents);
            _context.SaveChanges();
        }

        public void CreateRangeDocumentWorkflows(List<DocumentWorkflow> documentWorkflows)
        {
            _context.DocumentWorkflows.AddRange(documentWorkflows);
            _context.SaveChanges();
        }

        public int? GetLastBarcodeOfDocument()
        {
            return _context.Documents
               .AsNoTracking()
               .OrderByDescending(d => d.Id)
               .FirstOrDefault()?.Barcode;
        }

        public int GetDocumentsCount()
        {
            return _context.Documents
               .AsNoTracking()
               .Count();
        }
    }
}
