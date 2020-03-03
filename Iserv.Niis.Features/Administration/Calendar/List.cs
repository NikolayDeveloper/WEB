using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Calendar;
using Iserv.Niis.Model.Models.Calendar;
using MediatR;

namespace Iserv.Niis.Features.Administration.Calendar
{
    public class List
    {
        public class Query : IRequest<IQueryable<EventDto>>
        {
            public DateTimeOffset FromDate { get; set; }
            public DateTimeOffset ToDate { get; set; }
            public int EventTypeId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<EventDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            public async Task<IQueryable<EventDto>> Handle(Query message)
            {
                IQueryable<Event> events;
                if (message.EventTypeId > 0)
                    events = _context.Events.Where(e =>
                        e.Date >= message.FromDate
                        && e.Date <= message.ToDate
                        && e.EventTypeId == message.EventTypeId);
                else
                    events = _context.Events.Where(e =>
                        e.Date >= message.FromDate
                        && e.Date <= message.ToDate);

                return await Task.FromResult(events.ProjectTo<EventDto>());
            }
        }
    }
}