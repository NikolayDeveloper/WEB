using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Calendar;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Calendar
{
    public class GetCalendarEventsQuery: BaseQuery
    {
        public async Task<List<Event>> ExecuteAsync(DateTimeOffset fromDate, DateTimeOffset toDate, int? eventTypeId)
        {
            var eventsRepository = Uow.GetRepository<Event>();

            var eventQuery = eventsRepository.AsQueryable()
                .Where(e => e.Date >= fromDate
                            && e.Date <= toDate);
            if (eventTypeId != null)
                eventQuery = eventQuery.Where(e => e.EventTypeId == eventTypeId);

            return await eventQuery.ToListAsync();
        }
    }
}
