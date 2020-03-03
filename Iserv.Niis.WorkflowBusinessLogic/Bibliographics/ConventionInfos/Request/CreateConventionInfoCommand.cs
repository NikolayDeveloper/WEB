using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ConventionInfos.Request
{
    public class CreateConventionInfoCommand: BaseCommand
    {
        public int Execute(RequestConventionInfo info)
        {
            var repo = Uow.GetRepository<RequestConventionInfo>();
            repo.Create(info);
            Uow.SaveChanges();
            return info.Id;
        }
    }
}
