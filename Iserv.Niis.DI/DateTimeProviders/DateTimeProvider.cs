using Iserv.Niis.Domain.Enums;
using System;

namespace Iserv.Niis.DI.DateTimeProviders
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
        public DateTimeOffset NowStartDateTime => new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, TimeSpan.Zero);
        public DateTimeOffset NowEndDateTime => new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 23, 59, 59, TimeSpan.Zero);
    }
}
