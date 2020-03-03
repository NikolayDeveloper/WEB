using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataAccess.EntityFramework.Infrastructure;
using MediatR;

namespace Iserv.Niis.Features.Request
{
	public class Delete
	{
		public class Command : IRequest<Unit>
		{
			public Command(int requestId)
			{
				RequestId = requestId;
			}

			public int RequestId { get; }
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
				var requestId = message.RequestId;
				var request = _context.Requests.Find(requestId);
				if (request == null)
					throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
						 DataNotFoundException.OperationType.Delete, requestId);

				request.MarkAsDeleted();
				await _context.SaveChangesAsync();

				return await Unit.Task;
			}
		}
	}
}