
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicIPC
{
    public class GetDicIpcsByNameRuOrCodeQuery : BaseQuery
    {
        public async Task<List<Domain.Entities.Dictionaries.DicIPC>> ExecuteAsync(string searchText)
        {
            var lowerSearchText = searchText.ToLower();
            var ipcRepo = Uow.GetRepository<Domain.Entities.Dictionaries.DicIPC>();
            return await ipcRepo
                .AsQueryable()
                .Where(IsContainsText(lowerSearchText))
                .ToListAsync();
        }

        private Expression<Func<Domain.Entities.Dictionaries.DicIPC, bool>> IsContainsText(string searchText)
        {
            var searchTextLower = searchText.ToLower();
            return ipc => (ipc.NameRu != null && ipc.NameRu.ToLower().Contains(searchTextLower)) || ipc.Code.ToLower().Contains(searchTextLower);
        }
    }
}