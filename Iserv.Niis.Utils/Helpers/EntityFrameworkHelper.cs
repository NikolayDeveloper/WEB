using System.Linq;
using Iserv.Niis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Utils.Helpers
{
    public class EntityFrameworkHelper
    {
        public static void DetachAll(NiisWebContext context)
        {
            foreach (var entityEntry in context.ChangeTracker.Entries().ToArray())
                if (entityEntry.Entity != null)
                    entityEntry.State = EntityState.Detached;
        }
    }
}