using System;
using System.Linq;
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
    public class Update
    {
        public class Command : IRequest<SubjectDto>
        {
            public Owner.Type OwnerType { get; }
            public int Id { get; }
            public SubjectDto SubjectDto { get; }

            public Command(int id, SubjectDto subjectDto, Owner.Type ownerType)
            {
                Id = id;
                SubjectDto = subjectDto;
                OwnerType = ownerType;
            }
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
                var id = message.SubjectDto.Id = message.Id;
                var subjectDto = message.SubjectDto;

                var customer = _mapper.Map<SubjectDto, DicCustomer>(subjectDto);
                _customerUpdater.Update(customer);
                await _context.SaveChangesAsync();

                try
                {
                    switch (message.OwnerType)
                    {
                        case Owner.Type.Request:
                        {
                            var requestCustomer = _context.RequestCustomers.Find(id);

                            if (requestCustomer == null)
                                throw new DataNotFoundException(nameof(RequestCustomer),
                                    DataNotFoundException.OperationType.Update, (int) id);
                            _mapper.Map(subjectDto, requestCustomer);

                            await _context.SaveChangesAsync();

                            var requestCustomerWithIncludes = await _context.RequestCustomers.Include(rc => rc.CustomerRole)
                                .Include(rc => rc.Customer).ThenInclude(c => c.Type)
                                .Include(rc => rc.Customer).ThenInclude(c => c.CustomerAttorneyInfos)
                                .Include(rc => rc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                                .SingleAsync(rc => rc.Id == requestCustomer.Id);

                            return _mapper.Map<RequestCustomer, SubjectDto>(requestCustomerWithIncludes);
                            }
                        case Owner.Type.Contract:
                        {
                            var contractCustomer = _context.ContractCustomers.Find(id);

                            if (contractCustomer == null)
                                throw new DataNotFoundException(nameof(ContractCustomer),
                                    DataNotFoundException.OperationType.Update, (int) id);
                            _mapper.Map(subjectDto, contractCustomer);

                            await _context.SaveChangesAsync();
                            await _categoryIdentifier.IdentifyAsync(contractCustomer.ContractId);

                            var contractCustomerWithIncludes = await _context.ContractCustomers.Include(rc => rc.CustomerRole)
                                .Include(rc => rc.Customer).ThenInclude(c => c.Type)
                                .Include(rc => rc.Customer).ThenInclude(c => c.CustomerAttorneyInfos)
                                .Include(rc => rc.Customer).ThenInclude(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                                .SingleAsync(rc => rc.Id == contractCustomer.Id);

                            return _mapper.Map<ContractCustomer, SubjectDto>(contractCustomerWithIncludes);
                            }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception e)
                {
                    throw new DatabaseException(e.InnerException?.Message ?? e.Message);
                }
            }
        }
    }
}