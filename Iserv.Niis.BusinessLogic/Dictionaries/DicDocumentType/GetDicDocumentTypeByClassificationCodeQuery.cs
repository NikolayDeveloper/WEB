using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType
{
    //TODO: хрупкое решение, завязанное на значения code. Надо собирать список рекурсивно по внешним ключам
    public class GetDicDocumentTypeByClassificationCodeQuery : BaseQuery
    {
        public async Task<List<Domain.Entities.Dictionaries.DicDocumentType>> ExecuteAsync(string code)
        {
            var repository = Uow.GetRepository<Domain.Entities.Dictionaries.DicDocumentType>();
            var result = await repository.AsQueryable()
                .Where(r => r.Classification != null
                        && r.Classification.Code.StartsWith(code)
                        && r.Classification.Code != "01.01.01")
                        .ToListAsync();

            return result;
        }
    }
}
