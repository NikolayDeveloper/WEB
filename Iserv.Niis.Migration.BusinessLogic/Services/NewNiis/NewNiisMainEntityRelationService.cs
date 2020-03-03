using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisMainEntityRelationService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisMainEntityRelationService(NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangeMainEntityRelations(IEnumerable<object> mainEntityRelations)
        {
            _context.AddRange(mainEntityRelations);
            _context.SaveChanges();
        }

        public int? GetLastBarcode(Type entityType)
        {
            var payments = _context.Set(entityType) as IQueryable<Entity<int>>;
            return payments
                .AsNoTracking()
                .OrderByDescending(d => d.ExternalId)
                .FirstOrDefault()?.ExternalId;
        }
    }
}
