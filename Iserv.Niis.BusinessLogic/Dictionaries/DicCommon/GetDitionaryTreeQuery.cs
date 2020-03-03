using Iserv.Niis.Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCommon
{
    public class GetDitionaryTreeQuery : BaseQuery
    {
        public async Task<List<DictionaryEntity<int>>> ExecuteAsync(DictionaryType dictionaryType)
        {
            var repo = Uow.GetRepository();

            var entityClrType = repo.GetEntityClrType(dictionaryType.ToString());
            dynamic entity = Activator.CreateInstance(entityClrType);

            IQueryable dictionaries = repo.AsQueriable(entity);
            var query = await dictionaries.Cast<DictionaryEntity<int>>().AsQueryable().ToListAsync();

            return query.Where(r => r.GetType().GetProperty("ParentId")?.GetValue(r, null) == null).ToList();
        }
    }
}
