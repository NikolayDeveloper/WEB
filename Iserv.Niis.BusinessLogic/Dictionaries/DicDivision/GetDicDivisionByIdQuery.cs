using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDivision
{
    public class GetDicDivisionByIdQuery: BaseQuery
    {
        public Domain.Entities.Dictionaries.DicDivision Execute(int id)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDivision>();
            var division = repo.AsQueryable().FirstOrDefault(d => d.Id == id);

            return division;
        }
    }
}
