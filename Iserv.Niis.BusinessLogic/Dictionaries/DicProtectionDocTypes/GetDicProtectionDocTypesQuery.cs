using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.DataBridge.Implementations;
using System;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocTypes
{
    public class GetDicProtectionDocTypesQuery : BaseQuery
    {
        public async Task<List<DicProtectionDocType>> ExecuteAsync()
        {
            var protectionDocTypeRepo = Uow.GetRepository<DicProtectionDocType>();
            var result =  await protectionDocTypeRepo
                .AsQueryable()
                .Include(d => d.Route)
                .ToListAsync();
            return result;
        }
        //public async ValueTask<List<DicProtectionDocType>> ExecuteAsync(int i)
        //{
        //    var protectionDocTypeRepo = Uow.GetRepository<DicProtectionDocType>();
        //    try
        //    {
        //        var result = await protectionDocTypeRepo
        //            .AsQueryable()
        //            .Include(d => d.Route)
        //            .ToListAsync();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        var t = ex;
        //        return null;
        //    }

        //}
    }
}