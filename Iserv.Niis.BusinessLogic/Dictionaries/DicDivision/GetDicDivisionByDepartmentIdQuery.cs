using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicDivision
{
    public class GetDicDivisionByDepartmentIdQuery: BaseQuery
    {
        public Domain.Entities.Dictionaries.DicDivision Execute(int departmentId)
        {
            var repo = Uow.GetRepository<Domain.Entities.Dictionaries.DicDepartment>();
            var divisionId = repo.AsQueryable().Where(d => d.Id == departmentId).Select(d => d.DivisionId)
                .FirstOrDefault();

            return repo.AsQueryable()
                .Include(d => d.Division)
                .Select(d => d.Division)
                .FirstOrDefault(d => d.Id == divisionId);
        }
    }
}
