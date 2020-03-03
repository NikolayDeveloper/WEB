using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class UpdateRequestDocumentCommand : BaseCommand
    {
        public void Execute(RequestDocument requestDocument)
        {
            var repo = Uow.GetRepository<RequestDocument>();

            repo.Update(requestDocument);
            Uow.SaveChanges();
        }
    }
}