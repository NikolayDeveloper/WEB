using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using MediatR;

namespace Iserv.Niis.Features.Contract
{
    public class Delete
    {
        public class Command : IRequest<Unit>
        {
            public Command(int contractId)
            {
                ContractId = contractId;
            }

            public int ContractId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Unit>
        {
            private readonly NiisWebContext _context;

            public CommandHandler(NiisWebContext niisWebContext)
            {
                _context = niisWebContext;
            }

            public async Task<Unit> Handle(Command message)
            {
                var contractId = message.ContractId;
                var contract = _context.Contracts.Find(contractId);
                if (contract == null)
                    throw new DataNotFoundException(nameof(Domain.Entities.Contract.Contract),
                        DataNotFoundException.OperationType.Delete, contractId);

                await _context.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}