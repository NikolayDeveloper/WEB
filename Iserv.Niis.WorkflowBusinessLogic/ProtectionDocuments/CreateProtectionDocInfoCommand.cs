using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class CreateProtectionDocInfoCommand : BaseCommand
    {
        public ProtectionDoc Execute(int protectionDocId, ProtectionDocInfo info)
        {
            if (info != null)
            {
                var protectionDocRepository = Uow.GetRepository<ProtectionDocInfo>();
                info.ProtectionDocId = protectionDocId;
                protectionDocRepository.Create(info);
                Uow.SaveChanges();
            }

            return null;
        }
    }
}
