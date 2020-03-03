using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models.User;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Users
{
    public class GetExpertInfoByUserHandler : BaseHandler
    {
        public ExpertInfo Execute(ApplicationUser user)
        {
            var expertInfo = new ExpertInfo
            {
                UserId = user.Id,
                UserName = user.NameRu,
                IpcCodes = user.Ipcs.Any() ? string.Join(", ", user.Ipcs.Select(i => i.Ipc.Code)) : string.Empty
            };

            var requests = Executor.GetQuery<GetRequestsByUserIdQuery>().Process(q => q.Execute(user.Id));
            var sumCoefficientComplexity = requests.Where(r => r.CoefficientComplexity.HasValue).Sum(r => r.CoefficientComplexity.Value);
            expertInfo.EmploymentIndexExpert = Math.Round(sumCoefficientComplexity, 3);
            expertInfo.CountRequests = requests.Where(r => r.CurrentWorkflow.IsComplete == false).Count();
            expertInfo.CountCompletedRequestsCurrentYear = requests.Where(r => r.CurrentWorkflow.IsComplete == true && r.CurrentWorkflow.DateCreate.Year == DateTime.Now.Year).Count();
            expertInfo.RequestNumbers = string.Join(", ", requests.Where(r => string.IsNullOrEmpty(r.RequestNum) == false).Select(r => r.RequestNum));

            return expertInfo;
        }
    }
}
