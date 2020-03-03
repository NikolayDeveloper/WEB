using Iserv.Niis.BusinessLogic.Contracts;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.ProtectionDocs;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.BusinessLogic.Roles;
using Iserv.Niis.BusinessLogic.Workflows;
using Iserv.Niis.BusinessLogic.Workflows.Contracts;
using Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc;
using Iserv.Niis.BusinessLogic.Workflows.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Administration;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models;
using Iserv.Niis.Model.Models.Dictionaries;
using Iserv.Niis.Model.Models.Request;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowContracts;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using Iserv.Niis.WorkflowServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Dictionaries.DicRoutes;
using GetRequestByIdQuery = Iserv.Niis.WorkflowBusinessLogic.Requests.GetRequestByIdQuery;

namespace Iserv.Niis.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/workflow")]
    public class WorkflowController : BaseNiisApiController
    {
        [HttpGet("{ownerId}/{ownerType}")]
        public async Task<IActionResult> GetByParent(int ownerId, Owner.Type ownerType)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    return Ok(await GetRequestWorkflowsByOwnerId(ownerId));
                case Owner.Type.ProtectionDoc:
                    return Ok(await GetProtectionDocumentWorkflowsByOwnerId(ownerId));
                case Owner.Type.Contract:
                    return Ok(await GetContractWorkflowsByOwnerId(ownerId));
                default:
                    throw new ApplicationException(string.Empty, new ArgumentException($"{nameof(ownerType)}: {ownerType}"));
            }
        }

        // POST: api/Workflow
        [HttpPost("{ownerType}")]
        public async Task<IActionResult> Post([FromBody] WorkflowDto workflowDto, Owner.Type ownerType)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    return Ok(await ProcessRequestWorkflow(workflowDto) ?? throw GenerateProcessWorkflowException(nameof(ProcessRequestWorkflow)));
                case Owner.Type.ProtectionDoc:
                    return Ok(await ProcessProtectionDocumentWorkflow(workflowDto) ?? throw GenerateProcessWorkflowException(nameof(ProcessProtectionDocumentWorkflow)));
                case Owner.Type.Contract:
                    return Ok(await ProcessContractWorkflow(workflowDto) ?? throw GenerateProcessWorkflowException(nameof(ProcessContractWorkflow)));
                case Owner.Type.Material:
                    throw new Exception($"{nameof(WorkflowController)} => {nameof(Post)} not implemented owner type {ownerType}");
                case Owner.Type.None:
                    throw new Exception($"{nameof(WorkflowController)} => {nameof(Post)} implemented owner type is {ownerType}");
                default:
                    throw new Exception($"{nameof(WorkflowController)} => {nameof(Post)} not implemented owner type {ownerType}");
            }
        }

        [HttpGet("stageUsers/{stageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStageUsers(int stageId)
        {
            var workflowStageUsers = await Executor.GetQuery<GetWorkflowStageUsersQuery>().Process(q => q.ExecuteAsync(stageId));
            return Ok(workflowStageUsers);
        }

        [HttpGet("getBulletinUsers")]
        public async Task<IActionResult> GetBulletinUsers()
        {
            var roles = await Executor.GetQuery<GetRoleClaimsQuery>().Process(q => q.ExecuteAsync());
            var bulletinRoles = roles.Where(r => r.ClaimValue == ClaimValues.BulletinModule).Select(r => r.RoleId);

            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (var bulletinRoleId in bulletinRoles)
            {
                users.AddRange(Executor.GetQuery<GetUsersByRoleIdQuery>().Process(q => q.Execute(bulletinRoleId)));
            }
            var result = Mapper.Map<List<SelectOptionDto>>(users);

            return Ok(result);
        }

        [HttpGet("getSupportUsers")]
        public async Task<IActionResult> GetSupportUsers()
        {
            var roles = await Executor.GetQuery<GetRoleClaimsQuery>().Process(q => q.ExecuteAsync());
            var supportRoles = roles.Where(r => r.ClaimValue == ClaimValues.ProtectionDocSupport).Select(r => r.RoleId);

            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (var supportRoleId in supportRoles)
            {
                users.AddRange(Executor.GetQuery<GetUsersByRoleIdQuery>().Process(q => q.Execute(supportRoleId)));
            }
            var result = Mapper.Map<List<SelectOptionDto>>(users);

            return Ok(result);
        }

        [HttpGet("finishParallelWorkflow/{id}")]
        public async Task<IActionResult> FinishParallelWorkflow(int id)
        {
            var parallelWorkflow = await Executor.GetQuery<GetProtectionDocWorkflowByIdQuery>().Process(q => q.ExecuteAsync(id));
            if (parallelWorkflow is null)
                throw new Exception($"Parallel Workflow not found");
            Executor.GetCommand<WorkflowBusinessLogic.Workflows.FinishProtectionDocParallelWorkflowCommand>().Process(q => q.Execute(parallelWorkflow));

            return Ok(true);
        }


        [HttpGet("isParallelWorkflow/{id}")]
        public async Task<IActionResult> IsParallelWorkflow(int id)
        {
            var wrkf = await Executor.GetQuery<GetProtectionDocWorkflowByIdQuery>().Process(q => q.ExecuteAsync(id));
            if (wrkf is null)
                return Ok(false);
            var prWrkf = await Executor.GetCommand<WorkflowBusinessLogic.Workflows.GetProtectionDocParallelWorkflowByPDWorfIdCommand>().Process(q => q.ExecuteAsync(id));

            return Ok((wrkf.CurrentStage.Code != RouteStageCodes.OD03_1 && wrkf.CurrentStage.Code != RouteStageCodes.OD01_6) && prWrkf != null);
        }

        [HttpGet("getNextStagesByWorkflow/{workflowId}")]
        public async Task<IActionResult> GetNextStagesByWorkflow(int workflowId)
        {
            var routeId = Executor.GetQuery<GetDicRouteIdByWorkflowId>()
                .Process(q => q.Execute(workflowId));

            var nextStages = await Executor.GetQuery<GetAllRouteStageByRouteQuery>()
                .Process(q => q.ExecuteAsync(routeId ?? default(int)));

            //var nextStages = await Executor.GetQuery<GetDocumentNextStagesByDocumentCurrentWorkflowIdQuery>()
            //    .Process(q => q.ExecuteAsync(workflowId));

            return Ok(nextStages);
        }

        [HttpGet("getNextStages/{stageId}")]
        public async Task<IActionResult> GetNextStages(int stageId)
        {
            var routeId = Executor.GetQuery<GetDicRouteIdByStageId>()
                .Process(q => q.Execute(stageId));

            var nextStages = await Executor.GetQuery<GetAllRouteStageByRouteQuery>()
                .Process(q => q.ExecuteAsync(routeId ?? default(int)));

            //var nextStages = await Executor.GetQuery<GetNextStagesByCurrentStageIdQuery>()
            //    .Process(q => q.ExecuteAsync(stageId));

            return Ok(nextStages);
        }

        [HttpGet("getPreviousStages/{stageId}")]
        public async Task<IActionResult> GetPreviousStages(int stageId)
        {
            var previousStages = await Executor.GetQuery<GetPreviousStagesByCurrentStageIdQuery>()
                .Process(q => q.ExecuteAsync(stageId));

            return Ok(previousStages);
        }

        [HttpGet("getRouteStageById/{stageId}")]
        public async Task<IActionResult> GetRouteStageById(int stageId)
        {
            var stage = Executor.GetQuery<GetDicRouteStageByIdQuery>().Process(q => q.Execute(stageId));
            var result = Mapper.Map<DicRouteStageDto>(stage);

            return Ok(result);
        }

        [HttpGet("getRouteStageByCode/{stageCode}")]
        public async Task<IActionResult> GetRouteStageByCode(string stageCode)
        {
            var stage = await Executor.GetQuery<GetDicRouteStageByCodeQuery>().Process(q => q.ExecuteAsync(stageCode));
            var result = Mapper.Map<DicRouteStageDto>(stage);

            return Ok(result);
        }

        [HttpGet("requestStages/{requestId}")]
        public async Task<IActionResult> GetRequestStages(int requestId)
        {
            var stages = await Executor.GetQuery<GetDicRouteStageByRequestIdQuery>()
                .Process(q => q.ExecuteAsync(requestId));

            return Ok(stages);
        }

        [HttpGet("getNextStagesByOwner/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetNextStagesByRequestId(Owner.Type ownerType, int ownerId)
        {
            List<DicRouteStage> nextStages;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    nextStages = await Executor.GetHandler<FilterNextStagesByRequestIdHandler>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    break;
                case Owner.Type.ProtectionDoc:
                    nextStages = await Executor.GetHandler<FilterNextStagesByProtectionDocIdHandler>()
                        .Process(h => h.ExecuteAsync(ownerId));
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Ok(nextStages);
        }

        [HttpGet("getNextStagesByContractId/{contractId}")]
        public async Task<IActionResult> GetNextStagesByContractId(int contractId)
        {
            var nextStages = await Executor.GetQuery<GetFilterNextStagesByContractIdHandler>()
                .Process(h => h.ExecuteAsync(contractId));

            return Ok(nextStages);
        }

        [HttpGet("getPreviousStagesByOwner/{ownerType}/{ownerId}")]
        public async Task<IActionResult> GetPreviousStagesByRequestId(Owner.Type ownerType, int ownerId)
        {
            DicRouteStage previousStage;
            switch (ownerType)
            {
                case Owner.Type.Request:
                    previousStage = await Executor.GetQuery<FilterPreviousStagesByRequestIdHandler>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    break;
                case Owner.Type.ProtectionDoc:
                    previousStage = await Executor.GetQuery<FilterPreviousStagesByProtectionDocIdHandler>()
                        .Process(q => q.ExecuteAsync(ownerId));
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Ok(previousStage);
        }

        private async Task<IEnumerable<WorkflowDto>> GetContractWorkflowsByOwnerId(int ownerId)
        {
            //using NetCoreCQRS;
        //    var ex1 = new NetCoreCQRS.Executor(null);
        //    var t1 = ex1.GetQuery<GetContractWorkflowsByOwnerIdQuery>();
        //    var t2 = await t1.Process<ContractWorkflow, WorkflowDto>(async q => await q.ExecuteAsync(ownerId),
        //Mapper.Map<ContractWorkflow, WorkflowDto>);


        //    var contractWorkflowDtos = await Executor.GetQuery<GetContractWorkflowsByOwnerIdQuery>()
        //.Process<ContractWorkflow>(async q => await q.ExecuteAsync(ownerId));
            //return contractWorkflowDtos;


            var contractWorkflowDtos = await Executor.GetQuery<GetContractWorkflowsByOwnerIdQuery>()
                .Process<ContractWorkflow, WorkflowDto>(async q => await q.ExecuteAsync(ownerId),
                    Mapper.Map<ContractWorkflow, WorkflowDto>);
            return contractWorkflowDtos;
        }

        private async Task<IOrderedEnumerable<WorkflowDto>> GetProtectionDocumentWorkflowsByOwnerId(int ownerId)
        {
            var protectionDoc =
                await Executor.GetQuery<GetProtectionDocByIdQuery>().Process(q => q.ExecuteAsync(ownerId));

            // Новое требование: не наследовать маршруты заявки 
            //var requestWorkflowsDtos = (await Executor.GetQuery<GetRequestWorkflowsByOwnerIdQuery>()
            //    .Process<RequestWorkflow, WorkflowDto>(async q => await q.ExecuteAsync(protectionDoc?.RequestId ?? 0),
            //        Mapper.Map<RequestWorkflow, WorkflowDto>)).ToList();

            var protectionDocWorkflowDtos = await Executor.GetQuery<GetProtectionDocWorkflowsByOwnerIdQuery>()
                .Process<ProtectionDocWorkflow, WorkflowDto>(async q => await q.ExecuteAsync(ownerId),
                    Mapper.Map<ProtectionDocWorkflow, WorkflowDto>);

            var workflowDtos = protectionDocWorkflowDtos
                .Where(x => x.IsSystem.HasValue? !x.IsSystem.Value: true)
                .OrderByDescending(w => w.DateCreate.Ticks);
            return workflowDtos;
        }

        private async Task<IEnumerable<WorkflowDto>> GetRequestWorkflowsByOwnerId(int ownerId)
        {
            var workflowDtos = await Executor.GetQuery<GetRequestWorkflowsByOwnerIdQuery>()
                .Process<RequestWorkflow, WorkflowDto>(async q => await q.ExecuteAsync(ownerId),
                    Mapper.Map<RequestWorkflow, WorkflowDto>);
            return workflowDtos;
        }



        private async Task<WorkflowDto> ProcessRequestWorkflow(WorkflowDto workflowDto)
        {
            var requestWorkFlowRequest = new RequestWorkFlowRequest
            {
                RequestId = workflowDto.OwnerId ?? default(int),
                NextStageUserId = workflowDto.CurrentUserId ?? NiisAmbientContext.Current.User.Identity.UserId,
                NextStageCode = workflowDto.CurrentStageCode,
                PrevStageCode = workflowDto.FromStageCode,
            };

            NiisWorkflowAmbientContext.Current.RequestWorkflowService.Process(requestWorkFlowRequest);

            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(r => r.Execute(requestWorkFlowRequest.RequestId));
            await SetCoefficientComplexityRequest(request);
            //await CreatePaymentInvoicesForRequest(request);
            var requestWorkflow = await Executor.GetQuery<GetRequestWorkflowByIdQuery>().Process(r => r.ExecuteAsync(request.CurrentWorkflowId ?? default(int)));
            var responseWorkflowDto = Mapper.Map<RequestWorkflow, WorkflowDto>(requestWorkflow);

            return responseWorkflowDto;
        }

        private async Task<WorkflowDto> ProcessProtectionDocumentWorkflow(WorkflowDto workflowDto)
        {
            var protectionDocumentWorkFlowRequest = new ProtectionDocumentWorkFlowRequest
            {
                ProtectionDocId = workflowDto.OwnerId ?? default(int),
                NextStageUserId = workflowDto.CurrentUserId ?? default(int),
                NextStageCode = workflowDto.CurrentStageCode,
            };

            NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(protectionDocumentWorkFlowRequest);

            var protectionDoc = await Executor.GetQuery<GetProtectionDocByIdQuery>()
                .Process(q => q.ExecuteAsync(protectionDocumentWorkFlowRequest.ProtectionDocId));
            var protectionDocWorkflow = await Executor.GetQuery<GetProtectionDocWorkflowByIdQuery>().Process(r => r.ExecuteAsync(protectionDoc.CurrentWorkflowId ?? default(int)));
            var responseWorkflowDto = Mapper.Map<ProtectionDocWorkflow, WorkflowDto>(protectionDocWorkflow);
            return responseWorkflowDto;
        }

        private async Task<WorkflowDto> ProcessContractWorkflow(WorkflowDto workflowDto)
        {

            var contractWorkFlowRequest = new ContractWorkFlowRequest
            {
                ContractId = workflowDto.OwnerId ?? default(int),
                NextStageUserId = workflowDto.CurrentUserId ?? default(int),
                NextStageCode = workflowDto.CurrentStageCode,
            };

            NiisWorkflowAmbientContext.Current.ContractWorkflowService.Process(contractWorkFlowRequest);
            var contract = await Executor.GetQuery<GetContractByIdQuery>().Process(q => q.ExecuteAsync(contractWorkFlowRequest.ContractId));
            SetFullExpertiseExecutor(contract);
            SetApplicationDateCreate(contract);
            await CreatePaymentInvoicesForContact(contract);
            var contractWorkflow = await Executor.GetQuery<GetContractWorkflowByIdQuery>().Process(r => r.ExecuteAsync(contract.CurrentWorkflowId ?? default(int)));
            var responseWorkflowDto = Mapper.Map<ContractWorkflow, WorkflowDto>(contractWorkflow);

            return responseWorkflowDto;
        }

        private Exception GenerateProcessWorkflowException(string exceptionGeneratedMethodName)
        {
            return new Exception($"{nameof(WorkflowController)} -> {exceptionGeneratedMethodName} return null");
        }

        private void SetFullExpertiseExecutor(Contract contract)
        {
            var currentWorkflow = contract.CurrentWorkflow;
            if (currentWorkflow.FromStage.Code == RouteStageCodes.DK02_4 &&
                currentWorkflow.CurrentStage.Code == RouteStageCodes.DK02_5_1)
            {
                contract.FullExpertiseExecutorId = currentWorkflow.CurrentUserId;
                Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
            }
        }
        private async Task SetCoefficientComplexityRequest(Request request)
        {
            var routeStagesForCalculateCoefficient = new[] { RouteStageCodes.I_03_2_3, RouteStageCodes.I_03_2_3_0, RouteStageCodes.UM_03_1 };
            var requestWorkflow = request.CurrentWorkflow;
            if (routeStagesForCalculateCoefficient.Contains(requestWorkflow.CurrentStage.Code))
            {
                await Executor.GetHandler<SetCoefficientComplexityRequestHandler>().Process(h => h.ExecuteAsync(request.Id));
            }
        }

        private void SetApplicationDateCreate(Contract contract)
        {
            var currentWorkflow = contract.CurrentWorkflow;
            if (currentWorkflow.CurrentStage.Code == RouteStageCodes.DK02_1)
            {
                contract.ApplicationDateCreate = contract.IncomingDate;
                Executor.GetCommand<UpdateContractCommand>().Process(c => c.Execute(contract));
            }
        }

        private async Task CreatePaymentInvoicesForRequest(Request request)
        {
            if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_02_2)
            {
                var tariffCodesForI_02_2 = new List<string>();


                var beneficiaryType = request.RequestCustomers
                    .Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                    .Select(c => c.Customer.BeneficiaryType)
                    .FirstOrDefault();

                var isNotResident = request.RequestCustomers
                    .Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                    .Select(c => c.Customer.IsNotResident)
                    .FirstOrDefault();

                if (request.EarlyRegs != null && request.EarlyRegs.Any()
                    && beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB
                    && isNotResident == false)
                {
                    tariffCodesForI_02_2.Add(DicTariff.Codes.AcceptanceApplicationsConventionalPriorityAafterDeadline);
                }

                else if (request.ReceiveType != null && request.ReceiveType.Code == DicReceiveTypeCodes.Courier)
                {
                    if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB)
                    {
                        tariffCodesForI_02_2.Add(DicTariff.Codes.InventionFormalExaminationOnPurpose);
                    }
                    else if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.VET)
                    {
                        tariffCodesForI_02_2.Add(DicTariff.Codes.InventionAcceleratedFormalExaminationOnPurpose);
                    }
                }
                else if (request.ReceiveType != null && request.ReceiveType.Code == DicReceiveTypeCodes.ElectronicFeed
                                                    || request.ReceiveType.Code == DicReceiveTypeCodes.ElectronicFeedEgov)
                {
                    if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB)
                    {
                        tariffCodesForI_02_2.Add(DicTariff.Codes.InventionFormalExaminationEmail);
                    }
                    else if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.VET)
                    {
                        tariffCodesForI_02_2.Add(DicTariff.Codes.InventionAcceleratedFormalExaminationEmail);
                    }
                }


                if (tariffCodesForI_02_2.Any() == false)
                {
                    tariffCodesForI_02_2.Add(DicTariff.Codes.InventionFormalExaminationOnPurpose);
                }

                await Executor.GetHandler<CreatePaymentInvoicesHandler>()
                    .Process(h => h.Execute(Owner.Type.Request, request.Id, tariffCodesForI_02_2.ToArray(), DicPaymentStatusCodes.Notpaid, request.CurrentWorkflow.CurrentUserId));
            }

            if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.I_03_2_1)
            {
                var tariffCodesForI_03_2_1 = new List<string>
               {
                    DicTariff.Codes.ExaminationOfApplicationForInventionMerits
               };

                if (request.CountIndependentItems > 1)
                {
                    tariffCodesForI_03_2_1.Add(DicTariff.Codes.ExaminationEssentiallyAdditionallyIndependentClaimOverOne);
                }

                await Executor.GetHandler<CreatePaymentInvoicesHandler>()
                    .Process(h => h.Execute(Owner.Type.Request, request.Id, tariffCodesForI_03_2_1.ToArray(), DicPaymentStatusCodes.Notpaid, request.CurrentWorkflow.CurrentUserId));
            }

            if (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_02_2)
            {
                var tariffCodesForUM_02_2 = new List<string>();


                var beneficiaryType = request.RequestCustomers
                    .Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                    .Select(c => c.Customer.BeneficiaryType)
                    .FirstOrDefault();

                var isNotResident = request.RequestCustomers
                    .Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant)
                    .Select(c => c.Customer.IsNotResident)
                    .FirstOrDefault();

                if (request.EarlyRegs != null && request.EarlyRegs.Any()
                    && beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB
                    && isNotResident == false)
                {
                    tariffCodesForUM_02_2.Add(DicTariff.Codes.AcceptanceApplicationsConventionalPriorityAafterDeadline);
                }

                else if (request.ReceiveType != null && request.ReceiveType.Code == DicReceiveTypeCodes.Courier)
                {
                    if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.SMB)
                    {
                        tariffCodesForUM_02_2.Add(DicTariffCodes.UsefullModelPatentExaminationEmail);
                    }
                    else if (beneficiaryType != null && beneficiaryType.Code == DicBeneficiaryTypeCodes.VET)
                    {
                        tariffCodesForUM_02_2.Add(DicTariffCodes.UsefullModelPatentExaminationOnPurpose);
                    }
                }
                
                if (tariffCodesForUM_02_2.Any() == false)
                {
                    tariffCodesForUM_02_2.Add(DicTariffCodes.UsefullModelPatentExaminationOnPurpose);
                }

                await Executor.GetHandler<CreatePaymentInvoicesHandler>()
                    .Process(h => h.Execute(Owner.Type.Request, request.Id, tariffCodesForUM_02_2.ToArray(), DicPaymentStatusCodes.Notpaid, request.CurrentWorkflow.CurrentUserId));
            }
        }

        private async Task CreatePaymentInvoicesForContact(Contract contract)
        {
            if(contract.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.DK02_1_1)
            {
                var tariffCodes = new List<string>
                {
                    DicTariff.Codes._1030,
                };

                var relatedObjectsCount = contract.ProtectionDocs.Count() + contract.RequestsForProtectionDoc.Count();

                if(relatedObjectsCount > 1)
                {
                    tariffCodes.Add(DicTariff.Codes._34);
                }

                await Executor.GetHandler<CreatePaymentInvoicesHandler>()
                    .Process(h => h.Execute(Owner.Type.Contract, contract.Id, tariffCodes.ToArray(), DicPaymentStatusCodes.Notpaid, contract.CurrentWorkflow.CurrentUserId));

            }
        }
    }
}