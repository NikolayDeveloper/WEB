using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.SystemCounter
{
    public class UpdateSystemCounterCommand: BaseCommand
    {
        public object Execute(Domain.Entities.System.SystemCounter counter)
        {
            var repo = Uow.GetRepository<Domain.Entities.System.SystemCounter>();
            repo.Update(counter);

            Uow.SaveChanges();
            return null;
        }
    }
}
