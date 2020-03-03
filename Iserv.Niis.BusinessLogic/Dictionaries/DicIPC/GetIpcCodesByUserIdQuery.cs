using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Model.Models.Dictionaries.Ipc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicIPC
{
    public class GetIpcCodesByUserIdQuery : BaseQuery
    {
        public List<IpcCodeDetail> Execute(int userId)
        {
            var repo = Uow.GetRepository<UserIpcRelation>();

            var ipcCodes = repo
                .AsQueryable()
                .Include(ui => ui.Ipc)
                .Where(ui => ui.UserId == userId)
                .Select(ui => new IpcCodeDetail(ui.Ipc.Code ?? string.Empty))
                .ToList();

            return ipcCodes;
        }
    }
}
