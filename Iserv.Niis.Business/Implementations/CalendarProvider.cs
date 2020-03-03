using System;
using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Model.Models.Calendar;

namespace Iserv.Niis.Business.Implementations
{
    public class CalendarProvider : ICalendarProvider
    {
        private readonly NiisWebContext _context;
        private readonly string[] _dayOffTypes = { "Holiday", "Dayoff" };
        const string PublicationType = "Publication";

        public CalendarProvider(NiisWebContext context)
        {
            _context = context;
        }

        public DateTimeOffset GetPublicationDate(DateTimeOffset registerDate)
        {
            var publication = GetNextPublication(registerDate);

            if (publication == null)
            {
                throw new DataNotFoundException(nameof(Event),
                    DataNotFoundException.OperationType.Read, registerDate.ToString());
            }

            return publication.Date;
        }

        public PublicationRange GetPreviousPublicationRange(DateTimeOffset publicationDate)
        {
            var startPublication = GetPreviousPublication(publicationDate);
            var endPublication = GetPreviousPublication(startPublication.Date);

            if (startPublication == null || endPublication == null)
            {
                throw new DataNotFoundException(nameof(Event),
                    DataNotFoundException.OperationType.Read, publicationDate.ToString());
            }

            return new PublicationRange(startPublication.Date, endPublication.Date);
        }

        public DateTimeOffset GetExecutionDate(DateTimeOffset fromDate, ExpirationType expirationType, short expirationValue)
        {
            var executionDate = new DateTimeOffset();
            switch (expirationType)
            {
                case ExpirationType.CalendarDay:
                    {
                        executionDate = fromDate.AddDays(expirationValue);
                        break;
                    }
                case ExpirationType.WorkDay:
                    {
                        executionDate = fromDate.AddDays(expirationValue);
                        var holidayCount = GetHolidaysCount(fromDate, executionDate);

                        if (holidayCount > 0)
                        {
                            executionDate = executionDate.AddDays(holidayCount);
                        }
                        break;
                    }
                case ExpirationType.CalendarMonth:
                    {
                        executionDate = fromDate.AddMonths(expirationValue);
                        while (GetHolidaysCount(executionDate, executionDate) > 0)
                        {
                            executionDate = executionDate.AddDays(1);
                        }
                        break;
                    }
            }

            return executionDate;
        }

        public DateTimeOffset GetFormalExaminationDate(DateTimeOffset fromDate)
        {
            var accumulationDays = new[] { 1, 10, 20 };
            while (!accumulationDays.Contains(fromDate.Day))
            {
                fromDate = fromDate.AddDays(1);
            }

            var resultDate = fromDate.LocalDateTime.Date;
            resultDate = resultDate.AddHours(18);
            resultDate = resultDate.AddMinutes(30);
            while (IsHoliday(resultDate))
            {
                resultDate = resultDate.AddDays(1);
            }

            return resultDate;
        }

        public DateTimeOffset GetFullExaminationDate(DateTimeOffset fromDate)
        {
            var accumulationDays = new[] { 5, 15, 25 };
            while (!accumulationDays.Contains(fromDate.Day))
            {
                fromDate = fromDate.AddDays(1);
            }

            var resultDate = fromDate.LocalDateTime.Date;
            resultDate = resultDate.AddHours(18);
            resultDate = resultDate.AddMinutes(30);
            while (IsHoliday(resultDate))
            {
                resultDate = resultDate.AddDays(1);
            }

            return resultDate;
        }

        public DateTimeOffset GetTransferToGosreestrDate(DateTimeOffset fromDate)
        {
            var accumulationDays = new List<int> {
                15,
                fromDate.Month != 2 ? 30 : DateTime.DaysInMonth(fromDate.Year, fromDate.Month)
            };

            while (!accumulationDays.Contains(fromDate.Day))
            {
                fromDate = fromDate.AddDays(1);
            }

            var resultDate = fromDate.LocalDateTime.Date;
            resultDate = resultDate.AddHours(18);
            resultDate = resultDate.AddMinutes(30);
            while (IsHoliday(resultDate))
            {
                resultDate = resultDate.AddDays(1);
            }

            return resultDate;
        }

        public bool IsHoliday(DateTimeOffset date)
        {
            return _dayOffTypes.Contains(_context.Events.SingleOrDefault(e => e.Date == date)?.EventType?.Code);
        }

        #region Private Methods

        private Event GetPreviousPublication(DateTimeOffset selectedPublicationDate)
        {

            return _context.Events
                .OrderByDescending(e => e.Date)
                .FirstOrDefault(e =>
                    e.Date < selectedPublicationDate
                    && e.EventType.Code == PublicationType);
        }
        private Event GetNextPublication(DateTimeOffset registerDate)
        {
            return _context.Events
                .OrderByDescending(e => e.Date)
                .FirstOrDefault(e =>
                    e.Date > registerDate
                    && e.EventType.Code == PublicationType);
        }

        private int GetHolidaysCount(DateTimeOffset fromDate, DateTimeOffset executionDate)
        {
            return _context.Events.Count(e =>
                e.Date >= fromDate
                && e.Date <= executionDate
                && _dayOffTypes.Contains(e.EventType.Code));
        }
        #endregion
    }
}