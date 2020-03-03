using System.Linq;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestSelectionFromJournalHandler: BaseHandler
    {
        public int[] Execute(bool isAllSelected, SelectionMode selectionMode, int[] requestIds)
        {
            if (isAllSelected)
            {
                var requests = Executor.GetQuery<GetRequestsByUserIdQuery>()
                    .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));
                requestIds = requests.Select(r => r.Id).ToArray();
            }
            else
            {
                if (selectionMode == SelectionMode.Except)
                {
                    var requests = Executor.GetQuery<GetRequestsByUserIdQuery>()
                        .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));
                    requestIds = requests.Where(r => !requestIds.Contains(r.Id)).Select(r => r.Id).ToArray();
                }
            }
            return requestIds;
        }
    }
}
