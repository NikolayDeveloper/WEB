using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicProtectionDocTypes
{
    public class GetDicProtectionDocTypebyCodeQuery: BaseQuery
    {
        public DicProtectionDocType Execute(string code)
        {
            var repo = Uow.GetRepository<DicProtectionDocType>();

            return repo.AsQueryable().FirstOrDefault(r => r.Code == code);
        }
    }
}
