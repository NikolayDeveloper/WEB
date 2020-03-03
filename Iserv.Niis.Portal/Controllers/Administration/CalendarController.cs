using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Constants;
using Iserv.Niis.Features.Administration.Calendar;
using Iserv.Niis.Model.Models.Calendar;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Controllers.Administration
{
    [Produces("application/json")]
    [Route("api/Calendar")]
    [Authorize(Policy = KeyFor.Policy.HasAccessToAdministration)]
    public class CalendarController : Controller
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{fromDate}/{toDate}")]
        public async Task<IActionResult> Get(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            var query = new List.Query { FromDate = fromDate, ToDate = toDate };
            var events = await _mediator.Send(query);

            return Ok(events);
        }

        [HttpGet("GetByEventType/{fromDate}/{toDate}/{eventTypeId}")]
        public async Task<IActionResult> GetByEventType(DateTimeOffset fromDate, DateTimeOffset toDate, int eventTypeId)
        {
            var query = new List.Query { FromDate = fromDate, ToDate = toDate, EventTypeId = eventTypeId };
            var events = await _mediator.Send(query);

            return Ok(events);
        }

        [HttpGet("GetDateNow")]
        public async Task<IActionResult> GetDateNow()
        {
            return Ok(DateTimeOffset.Now);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ICollection<EventDto> eventDtos)
        {
            var savedEventDtos = await _mediator.Send(new Create.Command(eventDtos));

            return Ok(savedEventDtos);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new Delete.Command(id));

            return NoContent();
        }
    }
}
