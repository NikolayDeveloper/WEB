using AutoMapper.QueryableExtensions;
using Iserv.Niis.Domain.Entities.Bulletin;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.Model.Models.Bulletin;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class GetPagedBulletinSectionsQuery : BaseQuery
    {
        public IPagedList<BulletinSectionDto> Execute(HttpRequest httpRequest)
        {
            var bulletinSectionRepository = Uow.GetRepository<BulletinSection>();
            var sectionsQuery = bulletinSectionRepository
                .AsQueryable()
                .AsNoTracking()
                .ProjectTo<BulletinSectionDto>()
                .Filter(httpRequest.Query)
                .Sort(httpRequest.Query);

            return sectionsQuery.ToPagedList(httpRequest.GetPaginationParams());
        }
    }
}