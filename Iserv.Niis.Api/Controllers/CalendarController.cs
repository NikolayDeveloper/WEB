using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.BusinessLogic.Calendar;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.Model.Models.Calendar;
using Microsoft.AspNetCore.Mvc;
//using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; 
using NetCoreCQRS; 

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Calendar")]
    public class CalendarController : Controller
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public CalendarController(IExecutor executor, IMapper mapper)
        {
            _executor = executor;
            _mapper = mapper;
        }

        [HttpGet("{fromDate}/{toDate}")]
        public async Task<IActionResult> Get(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            var eventsList = await _executor.GetQuery<GetCalendarEventsQuery>().Process(q => q.ExecuteAsync(fromDate, toDate, null));
            var eventDtos = _mapper.Map<List<Event>, List<EventDto>>(eventsList);

            return Ok(eventDtos);
        }

        [HttpGet("GetByEventType/{fromDate}/{toDate}/{eventTypeId}")]
        public async Task<IActionResult> GetByEventType(DateTimeOffset fromDate, DateTimeOffset toDate, int eventTypeId)
        {
            var eventsList = await _executor.GetQuery<GetCalendarEventsQuery>().Process(q => q.ExecuteAsync(fromDate, toDate, eventTypeId));
            var eventDtos = _mapper.Map<List<Event>, List<EventDto>>(eventsList);

            return Ok(eventDtos);
        }

        [HttpGet("GetDateNow")]
        public async Task<IActionResult> GetDateNow()
        {
            return Ok(DateTimeOffset.Now);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]List<EventDto> eventDtos)
        {
            var eventList = _mapper.Map<List<EventDto>, List<Event>>(eventDtos);
            var eventsToProcess = eventList.Where(e => e.Date >= DateTimeOffset.Now.Date.ToUniversalTime());

            var createdEvents = await _executor.GetCommand<UpdateCalendarEventsRangeCommand>()
                .Process(c => c.ExecuteAsync(eventsToProcess.Where(e => e.Id > 0).ToList()));
            var updatedEvents = await _executor.GetCommand<CreateCalendarEventsRangeCommand>()
                .Process(c => c.ExecuteAsync(eventsToProcess.Where(e => e.Id == default(int)).ToList()));

            var newEventDtos = _mapper.Map<List<Event>, List<EventDto>>(createdEvents.Concat(updatedEvents).ToList());

            return Ok(newEventDtos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _executor.GetCommand<DeleteCalendarEventCommand>().Process(c => c.ExecuteAsync(id));

            return NoContent();
        }
    }
}