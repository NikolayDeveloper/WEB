using Iserv.Niis.BusinessLogic.ModulePayment.Dtos;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.ModulePayment
{
    public class GetPagedPaymentJournalDtoQuery : BaseQuery
    {
        public IPagedList<PaymentJournalDto> Execute(HttpRequest httpRequest)
        {
            var paymentRepository = Uow.GetRepository<Payment>();
            var paymentsQuery = paymentRepository
                .AsQueryable()
                .Include(r => r.PaymentUses)
                .Include(r => r.PaymentStatus)
                .Filter(httpRequest.Query)
                .Sort(httpRequest.Query);

            var requestParams = httpRequest.GetPaginationParams();

            var paymentPagedList = paymentsQuery.ToPagedList(requestParams);

            var result = PagedList<PaymentJournalDto>.Paginate(paymentPagedList.Items.Select(r => PaymentJournalDto.ToPaymentJournalDto(r)).AsQueryable(), requestParams.Limit, 1);
            result.Meta.TotalCount = paymentPagedList.Meta.TotalCount;

            return result;
        }
    }
}
