using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.NewNiis
{
    public class NewNiisSecurityUserService : BaseService
    {
        private readonly NiisWebContextMigration _context;

        public NewNiisSecurityUserService(
            NiisWebContextMigration context)
        {
            _context = context;
        }

        public void CreateRangeSecurityUsers(IEnumerable<object> securityUsers, Type entityType)
        {
            _context.AddRange(securityUsers);
            _context.SaveChanges();
        }

        public bool IsAnySecurityUsers(Type entityType)
        {
            var securityUsers = _context.Set(entityType) as IQueryable<dynamic>;
            return securityUsers.Any();
        }
    }
}
