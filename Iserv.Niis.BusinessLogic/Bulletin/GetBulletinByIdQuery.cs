using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bulletin
{
    public class GetBulletinByIdQuery: BaseQuery
    {
        public async Task<Domain.Entities.Bulletin.Bulletin> ExecuteAsync(int bulletinId)
        {
            var repo = Uow.GetRepository<Domain.Entities.Bulletin.Bulletin>();
            return await repo.GetByIdAsync(bulletinId);
        }
    }
}
