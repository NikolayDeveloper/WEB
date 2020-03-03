using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.AccountingData;
using Iserv.Niis.Model.Models.Payment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Payment
{
    public class List
    {
        public class Query : IRequest<IQueryable<PaymentDto>>
        {
            public string Xin { get; }

            public Query(string xin)
            {
                Xin = xin;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                // TODO: Валидация для ИИН
                RuleFor(p => p.Xin).Length(12);
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<PaymentDto>>
        {
            private readonly NiisWebContext _context;

            public QueryHandler(NiisWebContext context)
            {
                _context = context;
            }

            async Task<IQueryable<PaymentDto>> IAsyncRequestHandler<Query, IQueryable<PaymentDto>>.Handle(Query message)
            {
                var customer = await _context.DicCustomers.SingleOrDefaultAsync(c => c.Xin == message.Xin);

                if (customer == null)
                {
                    throw new DataNotFoundException(nameof(DicCustomer), DataNotFoundException.OperationType.Read, message.Xin);
                }

                var payments = _context.Payments
                    .Where(p => p.CustomerId == customer.Id);

                return payments.ProjectTo<PaymentDto>();
            }
        }
    }
}