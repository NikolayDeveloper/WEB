using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.Request
{
    public class CreateRequestIcgsCommand: BaseCommand
    {
        public int Execute(ICGSRequest icgs)
        {
            var repo = Uow.GetRepository<ICGSRequest>();
            repo.Create(icgs);
            Uow.SaveChanges();
            return icgs.Id;
        }
    }
}
