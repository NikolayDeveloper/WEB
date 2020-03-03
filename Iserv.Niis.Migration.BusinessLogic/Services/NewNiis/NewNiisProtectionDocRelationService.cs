using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System.Collections.Generic;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisProtectionDocRelationService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisProtectionDocRelationService(NiisWebContextMigration  context)
        {
            _context = context;
        }

        public void CreateRangeProtectionDocRelations(IEnumerable<object> protectionDocRelations)
        {
            _context.AddRange(protectionDocRelations);
            _context.SaveChanges();
        }
    }
}
