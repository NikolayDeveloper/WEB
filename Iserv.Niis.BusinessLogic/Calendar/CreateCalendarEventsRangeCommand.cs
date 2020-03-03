using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Calendar
{
    public class CreateCalendarEventsRangeCommand: BaseCommand
    {
        public async Task<List<Event>> ExecuteAsync(List<Event> eventList)
        {
            var eventsRepository = Uow.GetRepository<Event>();

            await eventsRepository.CreateRangeAsync(eventList);
            await Uow.SaveChangesAsync();

            return eventList;
        }
    }
}
