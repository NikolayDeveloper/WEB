using Iserv.Niis.BusinessLogic.Dictionaries.DicIPC;
using Iserv.Niis.BusinessLogic.Users;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.Model.Models.User;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class AllocateRequestsToExpertsHandler : BaseHandler
    {
        public List<IntellectualPropertyDto> Execute(List<IntellectualPropertyDto> intellectualPropertyDtos, int currentUserId)
        {
            var changeIntellectualPropertyDtos = new List<IntellectualPropertyDto>();
            var expertInfos = GetExpertInfos(currentUserId);

            foreach (var intellectualPropertyDto in intellectualPropertyDtos)
            {
                var fitExpertInfos = GetFitExpertsForRequest(intellectualPropertyDto.Id, expertInfos);
                if (fitExpertInfos.Any())
                {
                    var fitExpertInfo = GetExpertInfoWhereEmploymentIndexExpertMin(fitExpertInfos);
                    intellectualPropertyDto.ExpertId = fitExpertInfo.UserId;
                    RecountExpertInfo(expertInfos, fitExpertInfo.UserId, intellectualPropertyDto.CoefficientComplexity ?? default(double));
                }
                changeIntellectualPropertyDtos.Add(intellectualPropertyDto);
            }
            return changeIntellectualPropertyDtos;
        }

        private List<ExpertInfo> GetExpertInfos(int currentUserId)
        {
            var users = Executor.GetQuery<GetExpertsForAllocateQuery>().Process(q => q.Execute(currentUserId));
            var expertInfos = new List<ExpertInfo>();
            foreach (var user in users)
            {
                var expertInfo = Executor.GetHandler<GetExpertInfoByUserHandler>().Process(h => h.Execute(user));
                expertInfos.Add(expertInfo);
            }
            return expertInfos;
        }

        private List<ExpertInfo> GetFitExpertsForRequest(int requestId, List<ExpertInfo> expertInfos)
        {
            var fitExperts = new List<ExpertInfo>();
            var mainIpcCodeDetail = Executor.GetQuery<GetMainIpcCodeByRequestIdQuery>().Process(q => q.Execute(requestId));

            if (string.IsNullOrEmpty(mainIpcCodeDetail.SubClass) == false)
            {
                foreach (var expertInfo in expertInfos)
                {
                    if (IsFitExpertForRequestBySubClass(mainIpcCodeDetail.SubClass, expertInfo.UserId))
                    {
                        fitExperts.Add(expertInfo);
                    }
                }
            }

            if (string.IsNullOrEmpty(mainIpcCodeDetail.Class) == false && fitExperts.Any() == false)
            {
                foreach (var expertInfo in expertInfos)
                {
                    if (IsFitExpertForRequestByClass(mainIpcCodeDetail.Class, expertInfo.UserId))
                    {
                        fitExperts.Add(expertInfo);
                    }
                }
            }

            if (string.IsNullOrEmpty(mainIpcCodeDetail.Section) == false && fitExperts.Any() == false)
            {
                foreach (var expertInfo in expertInfos)
                {
                    if (IsFitExpertForRequestBySection(mainIpcCodeDetail.Section, expertInfo.UserId))
                    {
                        fitExperts.Add(expertInfo);
                    }
                }
            }
            return fitExperts;
        }

        private bool IsFitExpertForRequestBySubClass(string mainIpcCodeSubClass, int userId)
        {
            var ipcCodeDetails = Executor.GetQuery<GetIpcCodesByUserIdQuery>().Process(q => q.Execute(userId));
            return ipcCodeDetails.Select(i => i.SubClass).Contains(mainIpcCodeSubClass);
        }
        private bool IsFitExpertForRequestByClass(string mainIpcCodeClass, int userId)
        {
            var ipcCodeDetails = Executor.GetQuery<GetIpcCodesByUserIdQuery>().Process(q => q.Execute(userId));
            return ipcCodeDetails.Select(i => i.Class).Contains(mainIpcCodeClass);
        }
        private bool IsFitExpertForRequestBySection(string mainIpcCodeSection, int userId)
        {
            var ipcCodeDetails = Executor.GetQuery<GetIpcCodesByUserIdQuery>().Process(q => q.Execute(userId));
            return ipcCodeDetails.Select(i => i.Section).Contains(mainIpcCodeSection);
        }

        private void RecountExpertInfo(List<ExpertInfo> expertInfos, int userId, double coefficientComplexity)
        {
            expertInfos.Where(e => e.UserId == userId).ToList().ForEach(expertInfo => expertInfo.EmploymentIndexExpert += coefficientComplexity);
        }

        private ExpertInfo GetExpertInfoWhereEmploymentIndexExpertMin(List<ExpertInfo> expertInfos)
        {
            return expertInfos.OrderBy(e => e.EmploymentIndexExpert).First();
        }
    }
}
