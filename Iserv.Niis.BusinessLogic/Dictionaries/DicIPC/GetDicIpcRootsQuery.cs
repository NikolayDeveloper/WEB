using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicIPC
{
    public class GetDicIpcRootsQuery : BaseQuery
    {
        public async Task<List<Domain.Entities.Dictionaries.DicIPC>> ExecuteAsync()
        {
            var ipcRepo = Uow.GetRepository<Domain.Entities.Dictionaries.DicIPC>();
            return await ipcRepo
                .AsQueryable()
                .Where(i => i.Code.Length == 1)
                .ToListAsync();
        }
    }
}