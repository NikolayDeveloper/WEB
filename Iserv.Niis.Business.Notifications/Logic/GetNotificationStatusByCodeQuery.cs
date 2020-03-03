using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using NetCoreCQRS.Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetNotificationStatusByCodeQuery : BaseQuery
    {
        public DicNotificationStatus Execute(string statusCode)
        {
            return Uow.GetRepository<DicNotificationStatus>().AsQueryable()
                .SingleOrDefault(ns => ns.Code == statusCode);
        }
    }
}
