using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicDepartments
{
    public class GetDicDepartmentByIdQuery : BaseQuery
    {
        public DicDepartment Execute(int id)
        {
            return Uow.GetRepository<DicDepartment>().AsQueryable().FirstOrDefault(d => d.Id == id);
        }
    }
}
