using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Subject;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Customer
{
    public class AttachToOwner
    {
        public class Command : IRequest<SubjectDto>
        {
            public Command(SubjectDto subjectDto, Owner.Type ownerType)
            {
                SubjectDto = subjectDto;
                OwnerType = ownerType;
            }

            public SubjectDto SubjectDto { get; }
            public Owner.Type OwnerType { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
        }

        public class CommandHandler : IAsyncRequestHandler<Command, SubjectDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;
            private readonly ICustomerUpdater _customerUpdater;
            private readonly IContractCategoryIdentifier _categoryIdentifier;

            public CommandHandler(NiisWebContext context, IMapper mapper, ICustomerUpdater customerUpdater, IContractCategoryIdentifier categoryIdentifier)
            {
                _context = context;
                _mapper = mapper;
                _customerUpdater = customerUpdater;
                _categoryIdentifier = categoryIdentifier;
            }

            public async Task<SubjectDto> Handle(Command message)
            {
                var subjectDto = message.SubjectDto;
                var customer = _mapper.Map<SubjectDto, DicCustomer>(subjectDto);

                if (customer.Id == 0)
                {
                    throw new ValidationException("Unable to add power attorney!!!");
                }

                _customerUpdater.Update(customer);
                await _context.SaveChangesAsync();


                switch (message.OwnerType)
                {
                    case Owner.Type.Request:
                    {
                        var requestCustomer = _mapper.Map<SubjectDto, RequestCustomer>(subjectDto);
                        _context.RequestCustomers.Add(requestCustomer);
                        await _context.SaveChangesAsync();

                        var requestCustomerWithIncludes = await _context.RequestCustomers.Include(rc => rc.CustomerRole)
                            .Include(rc => rc.Customer).ThenInclude(c => c.Type)
                            .Include(rc => rc.Customer).ThenInclude(c => c.CustomerAttorneyInfos)
                            .SingleAsync(rc => rc.Id == requestCustomer.Id);

                        return _mapper.Map<RequestCustomer, SubjectDto>(requestCustomerWithIncludes);
                    }
                    case Owner.Type.Contract:
                    {
                        var contractCustomer = _mapper.Map<SubjectDto, ContractCustomer>(subjectDto);
                        _context.ContractCustomers.Add(contractCustomer);
                        await _context.SaveChangesAsync();
                        await _categoryIdentifier.IdentifyAsync(contractCustomer.ContractId);

                        var contractCustomerWithIncludes = await _context.ContractCustomers
                            .Include(cc => cc.CustomerRole)
                            .Include(cc => cc.Customer).ThenInclude(c => c.Type)
                            .Include(cc => cc.Customer).ThenInclude(c => c.CustomerAttorneyInfos)
                            .SingleAsync(cc => cc.Id == contractCustomer.Id);

                        return _mapper.Map<ContractCustomer, SubjectDto>(contractCustomerWithIncludes);
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}