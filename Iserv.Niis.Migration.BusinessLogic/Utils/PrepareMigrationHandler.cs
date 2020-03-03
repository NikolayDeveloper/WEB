using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.Common;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Migration.BusinessLogic.Utils
{
    public class PrepareMigrationHandler : BaseHandler
    {
        private readonly OldNiisContext _oldNiisContext;

        public PrepareMigrationHandler(
            NiisWebContextMigration niisWebContext,
            OldNiisContext oldNiisContext) : base(niisWebContext)
        {
            _oldNiisContext = oldNiisContext;
        }

        public void Execute()
        {
            var sqlCreateLogs = File.ReadAllText(Directory.GetCurrentDirectory() + "/CreateLogs.sql");
            NewNiisContext.Database.ExecuteSqlCommand(sqlCreateLogs);

            var sqlPrepareOldData = File.ReadAllText(Directory.GetCurrentDirectory() + "/PrepareOldData.sql");
            _oldNiisContext.Database.ExecuteSqlCommand(sqlPrepareOldData);
        }

        public void ExecuteScriptsByPath(string path)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var guid = Guid.NewGuid();
                try
                {
                    Log.LogInfo($"Начало выполнения скрипта {file} guid: {guid }");
                    var sqlCommand = File.ReadAllText(file);
                    NewNiisContext.Database.ExecuteSqlCommand(sqlCommand);
                    Log.LogInfo($"Успешно выполнен скрипт {file}. guid: {guid }");
                }
                catch (Exception ex)
                {
                    Log.LogError($"Ошибка при выполнении скрипта {file} guid: {guid }");
                    Log.LogError(ex);
                }
            }
        }
    }
}
