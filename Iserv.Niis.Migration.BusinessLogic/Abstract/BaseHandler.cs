using Iserv.Niis.Migration.Common;
using Iserv.OldNiis.DataAccess.EntityFramework;
using System;

namespace Iserv.Niis.Migration.BusinessLogic.Abstract
{
    public abstract class BaseHandler
    {
        protected readonly NiisWebContextMigration NewNiisContext;

        public BaseHandler(NiisWebContextMigration context)
        {
            NewNiisContext = context;
        }

        protected void ActionTransaction(Action dataSaveAction)
        {
            using (var transaction = NewNiisContext.Database.BeginTransaction())
            {
                try
                {
                    dataSaveAction();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Log.LogError(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
