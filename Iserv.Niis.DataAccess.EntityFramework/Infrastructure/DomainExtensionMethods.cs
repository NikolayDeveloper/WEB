using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.DataAccess.EntityFramework.Infrastructure
{
    public static class DomainExtensionMethods
    {
        public static void MarkAsDeleted(this ISoftDeletable softDeletable)
        {
            softDeletable.IsDeleted = true;
            softDeletable.DeletedDate = DateTimeOffset.Now;
        }
    }
}
