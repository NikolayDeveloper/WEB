using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class GetPaymentsQuery : BaseQuery
    {
        public IPagedList<PaymentDto> Execute(HttpRequest httpRequest)
        {
            var paymentRepository = Uow.GetRepository<Payment>();
            var paymentsQuery = paymentRepository
                .AsQueryable()
                .AsNoTracking()
                .ProjectTo<PaymentDto>()
                .Filter(httpRequest.Query)
                .Sort(httpRequest.Query);
            
            return paymentsQuery.ToPagedList(httpRequest.GetPaginationParams());
        }
    }
}
