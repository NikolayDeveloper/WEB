using System;
using System.Linq;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.Model.BusinessLogic
{
    public class CalculateExecutionDateHandler: BaseHandler
    {
        private readonly string[] _dayOffTypes = { "Holiday", "Dayoff" };
        public DateTimeOffset Execute(DateTimeOffset fromDate, ExpirationType expirationType, short expirationValue)
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

        private int GetHolidaysCount(DateTimeOffset fromDate, DateTimeOffset executionDate)
        {
            var events = Executor.GetQuery<GetCalendarEventsQuery>()
                .Process(q => q.Execute(fromDate, executionDate, null));

            return events.Count(e => _dayOffTypes.Contains(e.EventType.Code));
        }
    }
}
