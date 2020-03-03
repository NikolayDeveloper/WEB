using System;

namespace Iserv.Niis.DI.DateTimeProviders
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }

        DateTimeOffset NowStartDateTime { get; }

        DateTimeOffset NowEndDateTime { get; }
    }
}