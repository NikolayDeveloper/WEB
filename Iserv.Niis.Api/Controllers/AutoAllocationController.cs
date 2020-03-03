using System.Linq;
using Iserv.Niis.Infrastructure.Pagination;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.DI;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Iserv.Niis.Model.Models.Journal;
using System;
using Iserv.Niis.BusinessLogic.Users;
using System.Collections.Generic;
using Iserv.Niis.Model.Models.User;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowServices;
using Iserv.Niis.BusinessLogic.Workflows.Requests;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.BusinessLogic.Dictionaries.DicDocumentType;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.BusinessLogic.Documents;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Documents.Numbers;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.BusinessLogic.RequestDocumentRelations;
using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.Documents.UserInput;

namespace Iserv.Niis.Api.Controllers
{
    [Route("api/[controller]")]
    public class AutoAllocationController : BaseNiisApiController
    {
        [HttpGet]
        public IActionResult Get()
        {
            var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;

            var requests = Executor.GetQuery<GetRequestsByUserIdAndFilterAutoAllocationQuery>().Process(q => q.Execute(currentUserId));

            var requestDtos = Mapper.Map<IntellectualPropertyDto[]>(requests);

            var unionQuery = requestDtos.AsQueryable();

            var filteredQuery = unionQuery.Filter(Request.Query);
            var sortedQuery = filteredQuery;
            if (Request.Query.ContainsKey("_sort"))
            {
                sortedQuery = sortedQuery.Sort(Request.Query);
            }
            else
            {
                sortedQuery = sortedQuery.OrderByDescending(r => r.DateCreate);
            }

            var result = sortedQuery.ToPagedList(Request.GetPaginationParams());
            return result.AsOkObjectResult(Response);
        }

        [HttpGet("prepareAllocate/{requestIds}")]
        public IActionResult PrepareAllocate(string requestIds)
        {
            var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            var ids = requestIds.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
            var requests = Executor.GetQuery<GetRequestsByIdsQuery>().Process(q => q.Execute(ids));
            var intellectualPropertyDtos = Mapper.Map<IntellectualPropertyDto[]>(requests).ToList();
            var allocatedRequestDtos = Executor.GetHandler<AllocateRequestsToExpertsHandler>().Process(h => h.Execute(intellectualPropertyDtos, currentUserId));
            return Ok(allocatedRequestDtos);
        }

        [HttpPost("allocate")]
        public async Task<IActionResult> Allocate([FromBody] IntellectualPropertyDto[] intellectualPropertyDtos)
        {
            foreach (var intellectualPropertyDto in intellectualPropertyDtos)
            {
                if (intellectualPropertyDto.ExpertId.HasValue == false)
                {
                    continue;
                }
                var nextStageUserId = intellectualPropertyDto.ExpertId.Value;
                var nextStageCode = GetNextStageCode(intellectualPropertyDto.Id, nextStageUserId);

                var requestWorkFlowRequest = new RequestWorkFlowRequest
                {
                    RequestId = intellectualPropertyDto.Id,
                    NextStageUserId = nextStageUserId,
                    NextStageCode = nextStageCode
                };

                NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);
                await CreateDocumentResultDistributionRequests(requestWorkFlowRequest.RequestId, nextStageUserId);

            }
            return NoContent();
        }

        [HttpGet("getExperts")]
        public IActionResult GetExperts()
        {
            var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;
            var users = Executor.GetQuery<GetExpertsForAllocateQuery>().Process(q => q.Execute(currentUserId));
            var result = GetExpertInfoByUsers(users);
            return Ok(result);
        }

        private List<ExpertInfo> GetExpertInfoByUsers(List<ApplicationUser> users)
        {
            var expertInfos = new List<ExpertInfo>();
            foreach (var user in users)
            {
                var expertInfo = Executor.GetHandler<GetExpertInfoByUserHandler>().Process(h => h.Execute(user));
                expertInfos.Add(expertInfo);
            }
            return expertInfos;
        }

        private string GetNextStageCode(int requestId, int nextStageUserId)
        {
            var currentRequestWorkflow = Executor.GetQuery<GetCurrentWorkflowByRequestIdQuery>().Process(q => q.Execute(requestId));
            var nextStages = Executor.GetQuery<GetAllNextStagesByCurrentStageIdQuery>().Process(q => q.Execute(currentRequestWorkflow.CurrentStageId.Value));
            var currentStageCode = currentRequestWorkflow.CurrentStage.Code;
            var nextUser = Executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(nextStageUserId));

            var nextStageCode = string.Empty;
            if (currentStageCode == RouteStageCodes.I_03_2_3 && nextUser.Department.Division.Code == DicDivisionCodes.RGP_NIIS)
            {
                nextStageCode = nextStages.FirstOrDefault(s => s.Code == RouteStageCodes.I_03_2_2_1)?.Code;
            }
            if (currentStageCode == RouteStageCodes.I_03_2_3 && nextUser.Department.Division.Code == DicDivisionCodes.Filial_ALM_RGP_NIIS)
            {
                nextStageCode = nextStages.FirstOrDefault(s => s.Code == RouteStageCodes.I_03_2_2_0)?.Code;
            }
            if (currentStageCode == RouteStageCodes.I_03_2_3_0)
            {
                nextStageCode = nextStages.FirstOrDefault(s => s.Code == RouteStageCodes.I_03_2_4)?.Code;
            }
            if (currentStageCode == RouteStageCodes.UM_03_1)
            {
                nextStageCode = nextStages.FirstOrDefault(s => s.Code == RouteStageCodes.UM_03_2)?.Code;
            }

            if(string.IsNullOrEmpty(nextStageCode) == false)
            {
                return nextStageCode;
            }
            else
            {
                throw new Exception($"Not found next stage for requestId {requestId}");
            }            
        }

        private async Task CreateDocumentResultDistributionRequests(int requestId, int userId)
        {
            var docType = Executor.GetQuery<GetDicDocumentTypeByCodeQuery>().Process(q => q.Execute(DicDocumentTypeCodes.ResultDistributionRequests));
            var document = new Domain.Entities.Document.Document
            {
                DocumentType = DocumentType.Internal,
                TypeId = docType.Id,
            };
            var documentId = await Executor.GetCommand<CreateDocumentCommand>()
              .Process(q => q.ExecuteAsync(document));

            await Executor.GetHandler<GenerateDocumentBarcodeHandler>().Process(c => c.ExecuteAsync(documentId));
            await Executor.GetHandler<GenerateRegisterDocumentNumberHandler>().Process(c => c.ExecuteAsync(documentId));

            var requestDocument = new RequestDocument { RequestId = requestId, DocumentId = documentId };
            Executor.GetCommand<AddRequestDocumentCommand>().Process(c => c.Execute(requestDocument));
            var initialWorkflow = await Executor.GetQuery<GetInitialDocumentWorkflowQuery>()
                .Process(q => q.ExecuteAsync(documentId, userId));
            await Executor.GetCommand<ApplyDocumentWorkflowCommand>().Process(c => c.ExecuteAsync(initialWorkflow));
            var userInputDto = new Model.Models.Material.UserInputDto
            {
                Code = DicDocumentTypeCodes.ResultDistributionRequests,
                DocumentId = documentId,
                OwnerId = requestId,
            };
            await Executor.GetCommand<CreateUserInputCommand>().Process(c => c.ExecuteAsync(documentId, userInputDto));
        }
    }
}