using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.EarlyRegs.Request
{
    public class CreateRequestEarlyRegCommand: BaseCommand
    {
        public int Execute(RequestEarlyReg earlyReg)
        {
            var repo = Uow.GetRepository<RequestEarlyReg>();
            repo.Create(earlyReg);
            Uow.SaveChanges();
            return earlyReg.Id;
        }
    }
}
