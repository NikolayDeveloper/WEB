using System.Linq;
using Iserv.Niis.ExternalServices.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.Domain.Entities;

namespace Iserv.Niis.ExternalServices.Features.Utils
{
    public class LoggingHelper
    {
        private readonly NiisIntegrationContext _integrationContext;

        public LoggingHelper(NiisIntegrationContext integrationContext)
        {
            _integrationContext = integrationContext;
        }

        /// <summary>
        /// </summary>
        /// <param name="logSystemInfo"></param>
        /// <returns>Возвращает Id новой записи</returns>
        public int CreateLogSystemInfo(LogSystemInfo logSystemInfo)
        {
            _integrationContext.LogSystemInfos.Add(logSystemInfo);
            _integrationContext.SaveChanges();
            return logSystemInfo.Id;
        }

        public void CreateLogAction(LogAction logAction)
        {
            _integrationContext.LogActions.Add(logAction);
            _integrationContext.SaveChanges();
        }

        public void UpdateLogAction(LogAction logAction)
        {
            var action = _integrationContext.LogActions
                .SingleOrDefault(x => x.Id == logAction.Id);
            if (action != null)
            {
                action.SystemInfoAnswerId = logAction.SystemInfoAnswerId;
                action.Note = logAction.Note;
                _integrationContext.SaveChanges();
            }
        }

        public void CreateMonitorLog(bool isError, string note)
        {
            _integrationContext.IntegrationMonitorLogs.Add(new IntegrationMonitorLog
            {
                Error = isError,
                Note = note
            });
            _integrationContext.SaveChanges();
        }
    }
}