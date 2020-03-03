using System;
using System.Threading.Tasks;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using MediatR;

namespace Iserv.Niis.Features.Customer
{
    public class Delete
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; }
            public Owner.Type OwnerType { get; }

            public Command(int id, Owner.Type ownerType)
            {
                Id = id;
                OwnerType = ownerType;
            }
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
                var id = message.Id;
                
                try
                {
                    switch (message.OwnerType)
                    {
                        case Owner.Type.Request:
                        {
                            var requestCustomer = _context.RequestCustomers.Find(id);
                            if (requestCustomer == null)
                                throw new DataNotFoundException(nameof(RequestCustomer),
                                    DataNotFoundException.OperationType.Delete, id);
                            _context.RequestCustomers.Remove(requestCustomer);
                            break;
                        }
                        case Owner.Type.Contract:
                        {
                            var contractCustomer = _context.ContractCustomers.Find(id);
                            if (contractCustomer == null)
                                throw new DataNotFoundException(nameof(ContractCustomer),
                                    DataNotFoundException.OperationType.Delete, id);
                            _context.ContractCustomers.Remove(contractCustomer);
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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