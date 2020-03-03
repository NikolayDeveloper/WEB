using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Calendar
{
    public class DeleteCalendarEventCommand: BaseCommand
    {
        public async Task ExecuteAsync(int eventId)
        {
            var eventsRepository = Uow.GetRepository<Event>();
            var eventToDelete = await eventsRepository.GetByIdAsync(eventId);

            if (eventToDelete == null)
                throw new DataNotFoundException(nameof(Event),
                    DataNotFoundException.OperationType.Delete, eventId);

            eventsRepository.Delete(eventToDelete);
            await Uow.SaveChangesAsync();
        }
    }
}
