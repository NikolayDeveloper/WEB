using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetProtectionDocumentByIdQuery: BaseQuery
    {
        public ProtectionDoc Execute(int protectionDocId)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();

            return repo.GetById(protectionDocId);
        }
    }
}
