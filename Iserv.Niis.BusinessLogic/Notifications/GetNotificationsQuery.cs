using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Notifications
{
    public class GetNotificationsQuery : BaseQuery
    {
        public async Task<List<DicNotificationStatus>> ExecuteAsync(Owner.Type ownerType, int ownerId)
        {
            var repository = Uow.GetRepository<DicNotificationStatus>();
            List<DicNotificationStatus> result = new List<DicNotificationStatus>();
            switch (ownerType)
            {
                case Owner.Type.None:
                    result = null;
                    break;
                case Owner.Type.Request:
                    result = await repository.GetAll()
                        .AsQueryable()
                        .Where(n => n.Requests.Any(rn => rn.RequestId == ownerId))
                        .ToListAsync();
                    break;
                case Owner.Type.ProtectionDoc:
                    break;
                case Owner.Type.Contract:
                    result = await repository.GetAll()
                        .AsQueryable()
                        .Where(n => n.Contracts.Any(cn => cn.ContractId == ownerId))
                        .ToListAsync();
                    break;
                case Owner.Type.Material:
                    result = await repository.GetAll()
                        .AsQueryable()
                        .Where(n => n.Documents.Any(dn => dn.DocumentId == ownerId))
                        .ToListAsync();
                    break;
            }

            return result;
        }
    }
}
