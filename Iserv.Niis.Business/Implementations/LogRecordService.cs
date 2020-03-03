using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Other;
using System;

namespace Iserv.Niis.Business.Implementations
{
    public class LogRecordService : ILogRecordService
    {
        private readonly NiisWebContext _context;

        public LogRecordService(NiisWebContext context)
        {
            _context = context;
        }

        public void Log(LogRecord log)
        {
            log.DateCreate = DateTimeOffset.Now;
            _context.LogRecords.Add(log);
            _context.SaveChanges();
        }
    }
}