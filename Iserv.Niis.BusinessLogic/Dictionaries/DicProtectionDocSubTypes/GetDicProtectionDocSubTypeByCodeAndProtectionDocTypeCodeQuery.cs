using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicProtectionDocSubTypes
{
    public class GetDicProtectionDocSubTypeByCodeAndProtectionDocTypeCodeQuery: BaseQuery
    {
        public DicProtectionDocSubType Execute(string typeCode, string subTypeCode)
        {
            var repo = Uow.GetRepository<DicProtectionDocSubType>();

            return repo.AsQueryable()
                .FirstOrDefault(d => d.Code == subTypeCode && d.Type.Code == typeCode);
        }
    }
}
