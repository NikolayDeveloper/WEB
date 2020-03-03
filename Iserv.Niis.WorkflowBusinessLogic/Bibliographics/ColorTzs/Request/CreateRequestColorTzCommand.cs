using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.Request
{
    public class CreateRequestColorTzCommand: BaseCommand
    {
        public void Execute(DicColorTZRequestRelation colorTz)
        {
            var repo = Uow.GetRepository<DicColorTZRequestRelation>();
            repo.Create(colorTz);
            Uow.SaveChanges();
        }
    }
}
