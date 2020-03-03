using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.DataAccess.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Materials.Compare
{
    public class DocumentFinished
    {
        public class Command : IRequest<Unit>
        {
            public Command(int documentId)
            {
                DocumentId = documentId;
            }

            public int DocumentId { get; }
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

            public async Task<Unit> Handle(Command message)
            {
                var document = await _context.Documents
                    .FirstOrDefaultAsync(x => x.Id == message.DocumentId);
                if (document != null)
                {
                    document.IsFinished = true;
                    await _context.SaveChangesAsync();
                }
                return await Unit.Task;
            }
        }
    }
}