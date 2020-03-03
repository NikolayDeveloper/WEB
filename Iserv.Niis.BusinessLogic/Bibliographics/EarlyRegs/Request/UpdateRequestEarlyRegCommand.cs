using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request
{
    public class UpdateRequestEarlyRegCommand : BaseCommand
    {
        public void Execute(RequestEarlyReg earlyReg)
        {
            var repo = Uow.GetRepository<RequestEarlyReg>();
            repo.Update(earlyReg);
            Uow.SaveChanges();
        }
    }
}
