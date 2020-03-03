using Microsoft.EntityFrameworkCore;

namespace Iserv.OldNiis.DataAccess.EntityFramework.Utils
{
    public static class CustomSqlQuery
    {
        public static void SetTableIdentityInsertOn(NiisWebContextMigration context, string tableName)
        {
            context.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] ON", tableName));
        }

        public static void SetTableIdentityInsertOff(NiisWebContextMigration context, string tableName)
        {
            context.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] OFF", tableName));
        }

    }
}
