using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Info.ProtectionDocInfos
{
    public class CreateProtectionDocInfoCommand: BaseCommand
    {
        public void Execute(int protectionDocId, ProtectionDocInfo info)
        {
            if (info != null)
            {
                var protectionDocRepository = Uow.GetRepository<ProtectionDocInfo>();
                info.ProtectionDocId = protectionDocId;
                protectionDocRepository.Create(info);
                Uow.SaveChanges();
            }
        }
    }
}
