using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using MediatR;

namespace Iserv.Niis.Features.Materials
{
    public class Delete
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
                var documentId = message.DocumentId;
                var document = _context.Documents.Find(documentId);
                if (document == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Document.Document),
                        DataNotFoundException.OperationType.Delete, documentId);

                document.MarkAsDeleted();
                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
