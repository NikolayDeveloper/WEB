using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisProtectionDocService 
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisProtectionDocService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangeProtectionDocs(List<ProtectionDoc> protectionDocs)
        {
            _context.ProtectionDocs.AddRange(protectionDocs);
            _context.SaveChanges();
        }

        public int? GetLastBarcodeOfProtectionDoc()
        {
            return _context.ProtectionDocs
                 .AsNoTracking()
                 .OrderByDescending(d => d.Id)
                 .FirstOrDefault()?.Barcode;
        }

        public int GetProtectionDocsCount()
        {
            return _context.ProtectionDocs
                .AsNoTracking()
                .Count();
        }

        public void CreateRangeProtectionDocInfos(List<ProtectionDocInfo> protectionDocInfos)
        {
            _context.ProtectionDocInfos.AddRange(protectionDocInfos);
            _context.SaveChanges();
        }
    }
}
