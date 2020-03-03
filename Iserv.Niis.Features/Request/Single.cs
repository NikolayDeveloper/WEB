using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Model.Models.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Features.Request
{
    public class Single
    {
        public class Query : IRequest<RequestDetailDto>
        {
            public Query(int userId, Expression<Func<Domain.Entities.Request.Request, bool>> predicate)
            {
                UserId = userId;
                Predicate = predicate;
            }

            public int UserId { get; }
            public Expression<Func<Domain.Entities.Request.Request, bool>> Predicate { get; }
        }

        public class QueryValidator : AbstractValidator<Query> { }

        public class QueryHandler : IAsyncRequestHandler<Query, RequestDetailDto>
        {
            private readonly NiisWebContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(IMapper mapper, NiisWebContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            async Task<RequestDetailDto> IAsyncRequestHandler<Query, RequestDetailDto>.Handle(Query message)
            {
                var request = await _context.Requests
                    .Include(r => r.RequestInfo).ThenInclude(i => i.BreedCountry)
                    .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                    .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.FromStage)
                    .Include(r => r.ProtectionDocType)
                    .Include(r => r.RequestCustomers).ThenInclude(c => c.CustomerRole)
                    .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
                    .Include(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
                    .Include(r => r.Addressee)
                    .Include(r => r.ICGSRequests)
                    .Include(r => r.ICISRequests)
                    .Include(r => r.IPCRequests)
                    .Include(r => r.ColorTzs)
                    .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                    .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.PaymentUses)
                    .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                    .Include(r => r.EarlyRegs).ThenInclude(c => c.RegCountry)
                    .Include(r => r.Icfems)
                    .Include(r => r.RequestType)
                    .Include( r=> r.RequestConventionInfos)
                    .Include(r => r.Department)
                    .SingleOrDefaultAsync(message.Predicate);
                if (request == null)
                {
                    throw new DataNotFoundException(nameof(Domain.Entities.Request.Request),
                        DataNotFoundException.OperationType.Read, message.Predicate.ToString());
                }

                if (!request.IsRead && request.CurrentWorkflow?.CurrentUserId == message.UserId)
                {
                    request.IsRead = true;
                    _context.Entry(request).Property(x => x.IsRead).IsModified = true;
                    await _context.SaveChangesAsync();
                }

                return _mapper.Map<Domain.Entities.Request.Request, RequestDetailDto>(request,
                    opt => opt.Items["RequestCustomers"] = request.RequestCustomers);
            }
        }
    }
}