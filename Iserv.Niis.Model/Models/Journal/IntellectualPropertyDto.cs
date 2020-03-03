using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataBridge.Repositories;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.BusinessLogic;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
//using NetCoreDI;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Iserv.Niis.Model.Models.Journal
{
    public class IntellectualPropertyDto
    {
        public class ProtectionDocWithCurentUserId
        {
            public Domain.Entities.ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
            public long UserId { get; set; }
        }

        public int Id { get; set; }
        public string Barcode { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public string IncomingNumber { get; set; }
        public string IncomingNumberFilial { get; set; }
        public string Addressee { get; set; }
        public string ReceiveTypeNameRu { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public string ProtectionDocTypeValue { get; set; }
        public bool CanGenerateGosNumber { get; set; }
        public string CurrentStageValue { get; set; }
        public int ReviewDaysAll { get; set; }
        public int ReviewDaysStage { get; set; }
        public bool IsComplete { get; set; }
        public bool IsRead { get; set; }
        public Owner.Type OwnerType { get; set; }
        public string TaskType { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string RegNumber { get; set; }
        public string RequestNum { get; set; }
        public TaskPriority Priority { get; set; }
        public bool CanDownload { get; set; }
        public bool IsIndustrial { get; set; }
        public string IpcCodes { get; set; }
        public string IsMainIpcCode { get; set; }
        public int? PageCount { get; set; }
        public int? ExpertId { get; set; }
        public int? CountIndependentItems { get; set; }
        public double? CoefficientComplexity { get; set; }
        public bool? IsActiveProtectionDocument { get; set; }
        public ExpirationType ExpirationType { get; set; }
        public short? ExpirationValue { get; set; }
        public string CurrentStageCode { get; set; }
        public DateTimeOffset CurrentStageDate { get; set; }
        public DateTimeOffset? RequestDate { get; set; }
        public bool HasLegalDeclarants { get; set; }
        public int? SpeciesTradeMarkId { get; set; }
        public string MediaUrl { get; set; }
        public string SpeciesTrademarkCode { get; set; }
        public bool? IsFormalExamFeeNotPaidInTime { get; set; }
        public int? StatusId { get; set; }
        public string StatusValue { get; set; }
        public DateTimeOffset? RegistrationDate { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
        /// <summary>
        /// Заявитель
        /// </summary>
        public string Declarant { get; set; }
        /// <summary>
        /// Владелец
        /// </summary>
        public string DocumentOwner { get; set; }
        /// <summary>
        /// Патентный Поверенный
        /// </summary>
        public string PatentAttorney { get; set; }
        /// <summary>
        /// Доверенное лицо
        /// </summary>
        public string Confidant { get; set; }
        /// <summary>
        /// Адресат для переписки
        /// </summary>
        public string Correspondence { get; set; }
        /// <summary>
        /// Автор
        /// </summary>
        public string Author { get; set; }

        public string Transliteration { get; set; }

        public string NumberBulletin { get; set; }
        /// <summary>
        /// МКТУ 
        /// </summary>
        public string ICGS { get; set; }
        /// <summary>
        /// МКИЭТЗ Международная Классификация Изобразительных Элементов Товарных Знаков
        /// </summary>
        public string ICFEM { get; set; }

        /// <summary>
        /// МКПО Международная Классификация Промышленных Образцов.
        /// </summary>
        public string ICIS { get; set; }

        /// <summary>
        /// МПК Международная Патентная Классификация
        /// </summary>
        public string IPC { get; set; }

        public string User { get; set; }
        public string Executor { get; set; }


        public static Expression<Func<Domain.Entities.Request.Request, IntellectualPropertyDto>> MapFromRequest =
            request => new IntellectualPropertyDto
            {
                Id = request.Id,
                Barcode = request.Barcode.ToString(),
                IncomingNumber = request.IncomingNumber,
                DateCreate = request.DateCreate,
                Addressee = request.AddresseeId.HasValue
                    ? $"{request.Addressee.NameEn} {request.Addressee.NameRu} {request.Addressee.NameKz}"
                    : string.Empty,
                ReceiveTypeNameRu = request.ReceiveType != null ? request.ReceiveType.NameRu : string.Empty,
                ProtectionDocTypeId = request.ProtectionDocTypeId,
                ProtectionDocTypeValue =
                    request.ProtectionDocTypeId > 0 ? request.ProtectionDocType.NameRu : string.Empty,
                CanGenerateGosNumber = request.CurrentWorkflowId != null &&
                                       (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD01_1 ||
                                        request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PD_TM_AssignmentRegistrationNumber ||
                                        request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PD_NMPT_AssignmentRegistrationNumber),
                CurrentStageValue = request.CurrentWorkflowId.HasValue
                    ? request.CurrentWorkflow.CurrentStageId.HasValue
                        ? request.CurrentWorkflow.CurrentStage.NameRu
                        : string.Empty
                    : string.Empty,
                IsComplete = request.CurrentWorkflowId.HasValue &&
                             (request.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_03_4_0)
                              //&& request.Workflows.Any(rw => rw.CurrentStage.Code == RouteStageCodes.UM_03_9) 
                            || (request.CurrentWorkflow.IsComplete ?? false),
                IsRead = request.IsRead,
                OwnerType = Owner.Type.Request,
                TaskType = "request",
                NameRu = request.NameRu,
                NameEn = request.NameEn,
                NameKz = request.NameKz,
                Description = request.Description,
                RegNumber = request.RequestNum,
                CanDownload = request.MainAttachmentId.HasValue,
                IsIndustrial = new[]
                {
                    DicProtectionDocTypeCodes.RequestTypeInventionCode,
                    DicProtectionDocTypeCodes.RequestTypeUsefulModelCode,
                    DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode
                }.Contains(request.ProtectionDocType.Code),
                CountIndependentItems = request.CountIndependentItems,
                IsActiveProtectionDocument = false,
                ExpirationType = request.CurrentWorkflowId.HasValue
                    && request.CurrentWorkflow.CurrentStageId.HasValue
                    ? request.CurrentWorkflow.CurrentStage.ExpirationType
                    : ExpirationType.None,
                ExpirationValue = request.CurrentWorkflowId.HasValue
                    && request.CurrentWorkflow.CurrentStageId.HasValue
                    ? request.CurrentWorkflow.CurrentStage.ExpirationValue
                    : null,
                CurrentStageCode = request.CurrentWorkflowId.HasValue
                    && request.CurrentWorkflow.CurrentStageId.HasValue
                    ? request.CurrentWorkflow.CurrentStage.Code
                    : string.Empty,
                CurrentStageDate = request.CurrentWorkflowId.HasValue
                    ? request.CurrentWorkflow.DateCreate
                    : DateTimeOffset.Now,
                RequestDate = request.RequestDate,
                HasLegalDeclarants = request.RequestCustomers.Any(rc =>
                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                    rc.Customer.Type.Code == DicCustomerTypeCodes.Juridical),
                IsFormalExamFeeNotPaidInTime = request.IsFormalExamFeeNotPaidInTime,
                StatusId = request.StatusId,
                StatusValue = request.StatusId != null ? request.Status.NameRu : string.Empty,
                PublishDate = request.PublishDate,
                Declarant = request.RequestCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant).FirstOrDefault().Customer.NameRu,
                DocumentOwner = request.RequestCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Owner).FirstOrDefault().Customer.NameRu,
                PatentAttorney = request.RequestCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney).FirstOrDefault().Customer.NameRu,
                Confidant = request.RequestCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Confidant).FirstOrDefault().Customer.NameRu,
                Correspondence = request.RequestCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).FirstOrDefault().Customer.NameRu,
                Author = request.RequestCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Author).FirstOrDefault().Customer.NameRu,
                Transliteration = request.Transliteration,
                NumberBulletin = request.NumberBulletin,
                ICGS = string.Join("; ", request.ICGSRequests.Select(r => r.Icgs.NameRu)),
                ICFEM = string.Join("; ", request.Icfems.Select(r => r.DicIcfem.NameRu)),
                ICIS = string.Join("; ", request.ICISRequests.Select(i => i.Icis.NameRu)),
                IPC = string.Join("; ", request.IPCRequests.Select(i => i.Ipc.NameRu)),
                Executor = request.CurrentWorkflowId.HasValue ? request.CurrentWorkflow.CurrentUser.NameRu : string.Empty,
                User = request.User.NameRu
            };

        public static Expression<Func<Domain.Entities.Contract.Contract, IntellectualPropertyDto>> MapFromContract =
            contract => new IntellectualPropertyDto
            {
                Id = contract.Id,
                Barcode = contract.Barcode.ToString(),
                IncomingNumber = contract.IncomingNumber,
                DateCreate = contract.DateCreate,
                Addressee = contract.AddresseeId.HasValue
                    ? $"{contract.Addressee.NameEn} {contract.Addressee.NameRu} {contract.Addressee.NameKz}"
                    : string.Empty,
                ReceiveTypeNameRu = contract.ReceiveType != null ? contract.ReceiveType.NameRu : string.Empty,
                ProtectionDocTypeId = contract.ProtectionDocTypeId,
                ProtectionDocTypeValue =
                    contract.ProtectionDocTypeId > 0 ? contract.ProtectionDocType.NameRu : string.Empty,
                CanGenerateGosNumber = contract.CurrentWorkflowId != null &&
                                       (contract.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD01_1 ||
                                        contract.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PD_TM_AssignmentRegistrationNumber ||
                                        contract.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PD_NMPT_AssignmentRegistrationNumber),
                CurrentStageValue = contract.CurrentWorkflowId.HasValue
                    ? contract.CurrentWorkflow.CurrentStageId.HasValue
                        ? contract.CurrentWorkflow.CurrentStage.NameRu
                        : string.Empty
                    : string.Empty,
                IsComplete = contract.CurrentWorkflow.CurrentStage.IsLast,
                IsRead = contract.IsRead,
                OwnerType = Owner.Type.Contract,
                TaskType = "contract",
                NameRu = contract.NameRu,
                NameEn = contract.NameEn,
                NameKz = contract.NameKz,
                Description = contract.Description,
                RegNumber = contract.ContractNum,
                CanDownload = contract.MainAttachmentId.HasValue,
                IsIndustrial = new[]
                {
                    DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode,
                    DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode
                }.Contains(contract.ProtectionDocType.Code),
                IpcCodes = string.Empty,
                CountIndependentItems = 0,
                IsActiveProtectionDocument = false,
                ExpirationType = contract.CurrentWorkflowId.HasValue
                    && contract.CurrentWorkflow.CurrentStageId.HasValue
                    ? contract.CurrentWorkflow.CurrentStage.ExpirationType
                    : ExpirationType.None,
                ExpirationValue = contract.CurrentWorkflowId.HasValue
                    && contract.CurrentWorkflow.CurrentStageId.HasValue
                    ? contract.CurrentWorkflow.CurrentStage.ExpirationValue
                    : null,
                CurrentStageCode = contract.CurrentWorkflowId.HasValue
                    && contract.CurrentWorkflow.CurrentStageId.HasValue
                    ? contract.CurrentWorkflow.CurrentStage.Code
                    : string.Empty,
                CurrentStageDate = contract.CurrentWorkflowId.HasValue
                    ? contract.CurrentWorkflow.DateCreate
                    : DateTimeOffset.Now,
                RequestDate = DateTimeOffset.Now,
                HasLegalDeclarants = contract.ContractCustomers.Any(rc =>
                    rc.CustomerRole.Code == DicCustomerRoleCodes.Declarant &&
                    rc.Customer.Type.Code == DicCustomerTypeCodes.Juridical),
                StatusId = contract.StatusId,
                StatusValue = contract.StatusId != null ? contract.Status.NameRu : string.Empty,
                PublishDate = null,
                Declarant = contract.ContractCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant).FirstOrDefault().Customer.NameRu,
                DocumentOwner = contract.ContractCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Owner).FirstOrDefault().Customer.NameRu,
                PatentAttorney = contract.ContractCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney).FirstOrDefault().Customer.NameRu,
                Confidant = contract.ContractCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Confidant).FirstOrDefault().Customer.NameRu,
                Correspondence = contract.ContractCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).FirstOrDefault().Customer.NameRu,
                Author = contract.ContractCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Author).FirstOrDefault().Customer.NameRu,
                Transliteration = null,
                NumberBulletin = contract.NumberBulletin,
                ICGS = null,
                ICFEM = null,
                ICIS = null,
                IPC = null,
                Executor = contract.CurrentWorkflowId.HasValue ? contract.CurrentWorkflow.CurrentUser.NameRu : string.Empty,
                User = string.Empty
            };

        public static Expression<Func<ProtectionDocWithCurentUserId, IntellectualPropertyDto>>
            MapFromProtectionDocument = protectionDocWithCurentUserId => new IntellectualPropertyDto
            {
                Id = protectionDocWithCurentUserId.ProtectionDoc.Id,
                CurrentStageDate = protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.DateCreate,
                Barcode = protectionDocWithCurentUserId.ProtectionDoc.Barcode.ToString(),
                IncomingNumber = null,
                DateCreate = protectionDocWithCurentUserId.ProtectionDoc.DateCreate,
                Addressee = string.Empty,
                ReceiveTypeNameRu = string.Empty,
                ProtectionDocTypeId = protectionDocWithCurentUserId.ProtectionDoc.TypeId,
                ProtectionDocTypeValue = protectionDocWithCurentUserId.ProtectionDoc.Type.NameRu,
                CanGenerateGosNumber = protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflowId != null &&
                                       (protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD01_1 ||
                                        protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PD_TM_AssignmentRegistrationNumber ||
                                        protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.PD_NMPT_AssignmentRegistrationNumber) &&
                                       string.IsNullOrEmpty(protectionDocWithCurentUserId.ProtectionDoc.GosNumber),
                CurrentStageValue = GetCurrentStageValue(protectionDocWithCurentUserId),
                IsComplete = protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflowId != null &&
                             protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.IsComplete != null &&
                             protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.IsComplete.Value,
                IsRead = true,
                OwnerType = Owner.Type.ProtectionDoc,
                TaskType = "protectiondoc",
                NameRu = protectionDocWithCurentUserId.ProtectionDoc.NameRu,
                NameEn = protectionDocWithCurentUserId.ProtectionDoc.NameEn,
                NameKz = protectionDocWithCurentUserId.ProtectionDoc.NameKz,
                Description = protectionDocWithCurentUserId.ProtectionDoc.Description,
                RegNumber = protectionDocWithCurentUserId.ProtectionDoc.GosNumber,
                RequestNum = protectionDocWithCurentUserId.ProtectionDoc.RegNumber,
                CanDownload = false,
                CountIndependentItems = 0,
                IsIndustrial = protectionDocWithCurentUserId.ProtectionDoc.Type.Code == DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode ||
                               protectionDocWithCurentUserId.ProtectionDoc.Type.Code == DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode,
                IsActiveProtectionDocument = protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflowId != null &&
                                             protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow != null && 
                                             protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.CurrentStage != null &&
                                             (protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD05 ||
                                             protectionDocWithCurentUserId.ProtectionDoc.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.OD05_01),
                IsMainIpcCode = protectionDocWithCurentUserId.ProtectionDoc.IpcProtectionDocs.Any(ipc => ipc.Ipc != null && ipc.IsMain) ?
                    protectionDocWithCurentUserId.ProtectionDoc.IpcProtectionDocs.FirstOrDefault(ipc => ipc.IsMain && ipc.Ipc != null).Ipc.Code : null,
                StatusId = protectionDocWithCurentUserId.ProtectionDoc.StatusId,
                StatusValue = protectionDocWithCurentUserId.ProtectionDoc.StatusId != null ? protectionDocWithCurentUserId.ProtectionDoc.Status.NameRu : string.Empty,
                PublishDate = protectionDocWithCurentUserId.ProtectionDoc.PublishDate,
                Declarant = protectionDocWithCurentUserId.ProtectionDoc.ProtectionDocCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Declarant).FirstOrDefault().Customer.NameRu,
                DocumentOwner = protectionDocWithCurentUserId.ProtectionDoc.ProtectionDocCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Owner).FirstOrDefault().Customer.NameRu,
                PatentAttorney = protectionDocWithCurentUserId.ProtectionDoc.ProtectionDocCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.PatentAttorney).FirstOrDefault().Customer.NameRu,
                Confidant = protectionDocWithCurentUserId.ProtectionDoc.ProtectionDocCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Confidant).FirstOrDefault().Customer.NameRu,
                Correspondence = protectionDocWithCurentUserId.ProtectionDoc.ProtectionDocCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Correspondence).FirstOrDefault().Customer.NameRu,
                Author = protectionDocWithCurentUserId.ProtectionDoc.ProtectionDocCustomers.Where(c => c.CustomerRole.Code == DicCustomerRoleCodes.Author).FirstOrDefault().Customer.NameRu,
                RequestDate = null,
                Transliteration = protectionDocWithCurentUserId.ProtectionDoc.Transliteration,
                NumberBulletin = string.Join(";", protectionDocWithCurentUserId.ProtectionDoc.Bulletins.Select(b => b.Bulletin.Number)),
                ICGS = string.Join(";", protectionDocWithCurentUserId.ProtectionDoc.IcgsProtectionDocs.Select(r => r.Icgs.NameRu)),
                ICFEM = string.Join("; ", protectionDocWithCurentUserId.ProtectionDoc.Icfems.Select(r => r.DicIcfem.NameRu)),
                ICIS = string.Join("; ", protectionDocWithCurentUserId.ProtectionDoc.IcisProtectionDocs.Select(i => i.Icis.NameRu)),
                IPC = string.Join("; ", protectionDocWithCurentUserId.ProtectionDoc.IpcProtectionDocs.Select(i => i.Ipc.NameRu)),
                Executor = GetExecutor(protectionDocWithCurentUserId),
                User = string.Empty
            };

        private static string GetExecutor(ProtectionDocWithCurentUserId pdWithUserid )
        {
            try
            {
                var result = "";
                //return "А. Ашенова";
                //if (AmbientContext
                //        .Current
                //        .Resolver
                //        .ResolveObject<IExecutor>()
                //        .GetCommand<CheckProtectionDocParallelWorkflowOnFinishedCommand>()
                //        .Process(r => r.Execute(pdWithUserid.ProtectionDoc.Id)))
                //    return pdWithUserid.ProtectionDoc.CurrentWorkflowId.HasValue ? pdWithUserid.ProtectionDoc.CurrentWorkflow.CurrentUser.NameRu : string.Empty;
                var resolver = AmbientContext.Current.Resolver;
                var Executor = resolver.ResolveObject<IExecutor>();
                var command = Executor.GetCommand<CheckProtectionDocParallelWorkflowOnFinishedCommand>();
                var val1 = command.Process(r => r.Execute(pdWithUserid.ProtectionDoc.Id));

                if (val1)
                {
                    result = pdWithUserid.ProtectionDoc.CurrentWorkflowId.HasValue ? pdWithUserid.ProtectionDoc.CurrentWorkflow.CurrentUser.NameRu : string.Empty;
                    return result;
                }

                var resolver1 = AmbientContext.Current.Resolver;
                var Executor1 = resolver1.ResolveObject<IExecutor>();
                var command1 = Executor1.GetCommand<GetProtectionDocWorkflowFromParalleByOwnerIdCommand>();
                var workflow = command1.Process(r => r.Execute(pdWithUserid.ProtectionDoc.Id, pdWithUserid.UserId));

                //var workflow = AmbientContext
                //    .Current
                //    .Resolver
                //    .ResolveObject<IExecutor>()
                //    .GetCommand<GetProtectionDocWorkflowFromParalleByOwnerIdCommand>()
                //    .Process(r => r.Execute(pdWithUserid.ProtectionDoc.Id, pdWithUserid.UserId));
                result = workflow == null ?
                    string.Empty
                    : workflow.CurrentUserId.HasValue ?
                          workflow.CurrentUser.NameRu
                        : string.Empty;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private static string GetCurrentStageValue(ProtectionDocWithCurentUserId pdWithUserid)
        {
            try
            {
                var result = "";
               //return "Присвоение номера регистрации ТЗ";
                var pd = pdWithUserid.ProtectionDoc;

                if (pd.CurrentWorkflowId.HasValue && pd.CurrentWorkflow != null && pd.CurrentWorkflow.CurrentStageId.HasValue)
                {
                    if (pd.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.ODParallel)
                    {
                        var resolver = AmbientContext.Current.Resolver;
                        var Executor = resolver.ResolveObject<IExecutor>();
                        var command = Executor.GetCommand<GetProtectionDocWorkflowValueByParallelIdCommand>();

                        var paralletStagesValue = command.Process(r => r.Execute(pd.Id));

                        //var paralletStagesValue = AmbientContext
                        //                              .Current
                        //                              .Resolver
                        //                              .ResolveObject<IExecutor>()
                        //                              .GetCommand<GetProtectionDocWorkflowValueByParallelIdCommand>()
                        //                              .Process(r => r.Execute(pd.Id));
                        if (!string.IsNullOrEmpty(paralletStagesValue))
                        {
                            result = paralletStagesValue;
                            return result;
                        }
                        //return paralletStagesValue;
                    }
                    else
                    {

                        var resolver = AmbientContext.Current.Resolver;
                        var Executor = resolver.ResolveObject<IExecutor>();
                        var command = Executor.GetCommand<CheckProtectionDocParallelWorkflowOnFinishedCommand>();
                        var val1 = command.Process(r => r.Execute(pd.Id));
                        //if (AmbientContext
                        //    .Current
                        //    .Resolver
                        //    .ResolveObject<IExecutor>()
                        //    .GetCommand<CheckProtectionDocParallelWorkflowOnFinishedCommand>()
                        //    .Process(r => r.Execute(pd.Id)))
                        //{
                        if (val1)
                        {
                            if (!string.IsNullOrEmpty(pd.CurrentWorkflow.CurrentStage.NameRu))
                            {
                                result = pd.CurrentWorkflow.CurrentStage.NameRu;
                                return result;

                            }
                            //return pd.CurrentWorkflow.CurrentStage.NameRu;
                        }
                        else
                        {
                            var resolver1 = AmbientContext.Current.Resolver;
                            var Executor1 = resolver1.ResolveObject<IExecutor>();
                            var command1 = Executor1.GetCommand<GetProtectionDocWorkflowValueByParallelIdCommand>();

                            var paralletStagesValue = command1.Process(r => r.Execute(pd.Id));


                            //var paralletStagesValue = AmbientContext
                            //                          .Current
                            //                          .Resolver
                            //                          .ResolveObject<IExecutor>()
                            //                          .GetCommand<GetProtectionDocWorkflowValueByParallelIdCommand>()
                            //                          .Process(r => r.Execute(pd.Id));

                            if (string.IsNullOrEmpty(paralletStagesValue))
                            {
                                result = pd.CurrentWorkflow.CurrentStage.NameRu;
                                return result;
                            }
                            else
                            {
                                result = string.Join(" | ", pd.CurrentWorkflow.CurrentStage.NameRu, paralletStagesValue);
                                return result;
                            }
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}