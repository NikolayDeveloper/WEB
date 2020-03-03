using Iserv.Niis.Domain.Entities.Other;

namespace Iserv.Niis.Business.Abstract
{
    public interface ILogRecordService
    {
        void Log(LogRecord log);
    }
}