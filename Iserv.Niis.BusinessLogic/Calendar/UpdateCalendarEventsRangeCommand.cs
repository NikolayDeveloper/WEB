using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Calendar;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Calendar
{
    public class UpdateCalendarEventsRangeCommand: BaseCommand
    {
        public async Task<List<Event>> ExecuteAsync(List<Event> eventList)
        {
            var eventsRepository = Uow.GetRepository<Event>();

            foreach (var newEvent in eventList)
            {
                //Перенес как есть, почему либо по айдишнику либо по дате - непонятно, надо в постановке смотреть
                var oldEvent = await eventsRepository.AsQueryable()
                    .FirstOrDefaultAsync(e => e.Id == newEvent.Id || e.Date == newEvent.Date);
                if (oldEvent != null)
                {
                    oldEvent.EventTypeId = newEvent.EventTypeId;
                    eventsRepository.Update(oldEvent);
                }
            }
            await Uow.SaveChangesAsync();

            return eventList;
        }
    }
}
