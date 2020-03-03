using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Model.Models.Subject;
using MediatR;

namespace Iserv.Niis.Features.Customer
{
    public class Create
    {
        public class Command : IRequest<SubjectDto>
        {
            public Command(SubjectDto subjectDto)
            {
                SubjectDto = subjectDto;
            }

            public SubjectDto SubjectDto { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, SubjectDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(NiisWebContext context, IMapper mapper, ICustomerUpdater customerUpdater)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<SubjectDto> Handle(Command message)
            {
                var subjectDto = message.SubjectDto;
                var customer = _mapper.Map<SubjectDto, DicCustomer>(subjectDto);

                if (customer.Xin != null && !customer.Xin.Equals(string.Empty) &&
                    _context.DicCustomers.Any(dc => dc.Xin.Equals(customer.Xin)))
                {
                    throw new ValidationException($"Customer with XIN {customer.Xin} already exists");
                }
                if (_context.DicCustomers.Any(dc => dc.NameRu == customer.NameRu))
                {
                    throw new ValidationException($"Customer with such data already exists!");
                }
                await _context.DicCustomers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return _mapper.Map<SubjectDto>(customer);
            }
        }
    }
}

