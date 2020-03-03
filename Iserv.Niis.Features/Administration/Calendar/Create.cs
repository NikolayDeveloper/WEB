using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.Model.Models.Calendar;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Iserv.Niis.Features.Administration.Calendar
{
    public class Create
    {
        public class Command : IRequest<ICollection<EventDto>>
        {
            public Command(ICollection<EventDto> calendarEvents)
            {
                Events = calendarEvents;
            }
            public ICollection<EventDto> Events { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Events.Select(e => e.Date)).NotEmpty();
                RuleFor(x => x.Events.Select(e => e.EventTypeId)).NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, ICollection<EventDto>>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(
                NiisWebContext context,
                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ICollection<EventDto>> Handle(Command message)
            {
                var @events = _mapper.Map<ICollection<Event>>(message.Events.Where(e=>e.Date > DateTimeOffset.Now));

                var oldEvents = message.Events
                    .Where(e => e.Id > 0)
                    .OrderBy(e=>e.Id)
                    .ToList();

                var existingEvents = await _context.Events
                    .Where(e => oldEvents.Select(oe => oe.Id).Contains(e.Id) || events.Select(oe => oe.Date).Contains(e.Date))
                    .OrderBy(e => e.Id)
                    .ToListAsync();

                foreach (var existingEvent in existingEvents)
                {
                    var modifiedEvent = oldEvents.FirstOrDefault(e=>e.Id == existingEvent.Id || e.Date == existingEvent.Date);
                    if (modifiedEvent != null && existingEvent.EventTypeId != modifiedEvent.EventTypeId)
                    {
                        existingEvent.EventTypeId = modifiedEvent.EventTypeId;
                    }
                    _context.Events.Attach(existingEvent);
                } 
                var notExistEvents = @events.Where(e => e.Id < 1 && !existingEvents.Select(oe => oe.Date).Contains(e.Date)).ToList();
                
                try
                {
                    _context.Events.AddRange(notExistEvents);
                    await _context.SaveChangesAsync();

                    var savedEvent = existingEvents.Union(notExistEvents);
                    return _mapper.Map<ICollection<EventDto>>(savedEvent);
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }
        }
    }
}