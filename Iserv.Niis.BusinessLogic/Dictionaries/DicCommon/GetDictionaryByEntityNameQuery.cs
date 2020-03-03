using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCommon
{
    public class GetDictionaryByEntityNameQuery : BaseQuery
    {
        public async Task<List<dynamic>> ExecuteAsync(DictionaryType dictionaryType)
        {
            var repo = Uow.GetRepository();

            var entityClrType = repo.GetEntityClrType(dictionaryType.ToString());
            dynamic entity = Activator.CreateInstance(entityClrType);

            IQueryable dictionary = repo.AsQueriable(entity);
            return await dictionary.Cast<dynamic>().ToListAsync();
        }
    }
}