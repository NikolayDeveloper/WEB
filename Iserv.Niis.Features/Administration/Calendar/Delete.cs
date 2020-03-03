using System;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using MediatR;

namespace Iserv.Niis.Features.Administration.Calendar
{
    public class Delete
    {
        public class Command : IRequest<Unit>
        {
            public Command(int eventId)
            {
                EventId = eventId;
            }

            public int EventId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Unit>
        {
            private readonly NiisWebContext _context;

            public CommandHandler(NiisWebContext context)
            {
                _context = context;
            }

            public async  Task<Unit> Handle(Command message)
            {
                var existEvent = _context.Events.Find(message.EventId);
                if (existEvent == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Calendar.Event),
                        DataNotFoundException.OperationType.Delete, message.EventId);

                _context.Events.Remove(existEvent);

                try
                {
                    await _context.SaveChangesAsync();
                    return await Unit.Task;
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }
        }
    }
}