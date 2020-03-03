using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Dictionaries
{
    public class GetDicTariffByCodeQuery: BaseQuery
    {
        public DicTariff Execute(string code)
        {
            var repo = Uow.GetRepository<DicTariff>();
            return repo.AsQueryable()
                .FirstOrDefault(d => d.Code == code);
        }
    }
}
