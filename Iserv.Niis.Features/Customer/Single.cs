using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Features.Helpers;
using Iserv.Niis.Model.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Customer
{
    public class Single
    {
        public class Query : IRequest<CustomerShortInfoDto>
        {
            public Query(string xin, bool? isPatentAttorney)
            {
                Xin = xin;
                IsPatentAttorney = isPatentAttorney;
            }
            public string Xin { get; }
            public bool? IsPatentAttorney { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(c => c.Xin)
                    .NotEmpty()
                    .Length(12)
                    .IsCorrectXinFormat();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, CustomerShortInfoDto>
        {
            private readonly IMapper _mapper;
            private readonly NiisWebContext _context;
            private readonly ICustomerUpdater _customerUpdater;

            public QueryHandler(IMapper mapper, NiisWebContext context, ICustomerUpdater customerUpdater)
            {
                _mapper = mapper;
                _context = context;
                _customerUpdater = customerUpdater;
            }

            async Task<CustomerShortInfoDto> IAsyncRequestHandler<Query, CustomerShortInfoDto>.Handle(Query message)
            {
                var xin = message.Xin;

                var customer = _context.DicCustomers
                    .Include(c => c.Type)
                    .Include(c => c.CustomerAttorneyInfos)
                    .Include(c => c.ContactInfos).ThenInclude(ci => ci.Type)
                    .Where(r => r.Xin.Equals(xin) &&
                                (message.IsPatentAttorney == null || (message.IsPatentAttorney.Value
                                     ? r.PowerAttorneyFullNum != null
                                     : r.PowerAttorneyFullNum == null)));
                if ((customer == null || !customer.Any()) && message.IsPatentAttorney != null)
                    throw new DataNotFoundException(nameof(DicCustomer), DataNotFoundException.OperationType.Read,
                        message.Xin);

                if (customer.Count() > 1 && message.IsPatentAttorney != null)
                    throw new ValidationException($"Argument XIN with {xin} value has multiple match!");

                return _mapper.Map<DicCustomer, CustomerShortInfoDto>(await customer.FirstAsync());

                // todo использовать, когда будет доступ к ГБД ФЛ/ЮЛ
                // return _mapper.Map<DicCustomer, CustomerShortInfoDto>(await _customerUpdater.GetCustomer(xin, message.IsPatentAttorney));
            }
        }
    }
}