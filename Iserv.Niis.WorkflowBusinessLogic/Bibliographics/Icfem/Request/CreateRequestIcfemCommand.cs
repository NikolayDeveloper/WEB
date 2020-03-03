using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.Request
{
    public class CreateRequestIcfemCommand: BaseCommand
    {
        public void Execute(DicIcfemRequestRelation icfem)
        {
            var repo = Uow.GetRepository<DicIcfemRequestRelation>();
            repo.Create(icfem);
            Uow.SaveChanges();
        }
    }
}
