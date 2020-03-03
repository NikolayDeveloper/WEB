using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDetailICGS
{
    public class GetDicDetailICGSByIcgsIdQuery : BaseQuery
    {
        public async Task<List<Domain.Entities.Dictionaries.DicDetailICGS>> ExecuteAsync(int icgsId)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDetailICGS>();

            return await repo.AsQueryable().Where(r => r.IcgsId == icgsId).ToListAsync();            
        }
    }
}
