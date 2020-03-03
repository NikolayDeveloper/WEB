using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataBridge.Repositories;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.BusinessLogic;
using Iserv.Niis.Model.Models.Journal;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Model.Mappings.Request
{
    public class RequestDtoProfile : Profile
    {
        public RequestDtoProfile()
        {
            CreateMap<Domain.Entities.Request.Request, IntellectualPropertyDto>()
                .ForMember(dest => dest.CurrentStageValue,
                    opt => opt.MapFrom(src => src.CurrentWorkflowId.HasValue
                        ? src.CurrentWorkflow.CurrentStageId.HasValue
                            ? src.CurrentWorkflow.CurrentStage.NameRu
                            : string.Empty
                        : string.Empty))
                .ForMember(dest => dest.Addressee, src => src.MapFrom(x =>
                    x.AddresseeId.HasValue
                        ? $"{x.Addressee.NameEn} {x.Addressee.NameRu} {x.Addressee.NameKz}"
                        : string.Empty))
                .ForMember(dest => dest.ProtectionDocTypeValue, src => src.MapFrom(x =>
                    x.ProtectionDocTypeId > 0
                        ? x.ProtectionDocType.NameRu
                        : string.Empty))
                .ForMember(dest => dest.ReviewDaysAll,
                    opt => opt.MapFrom(src => (DateTimeOffset.Now - src.DateCreate).Days))
                .ForMember(dest => dest.ReviewDaysStage,
                    opt => opt.MapFrom(src =>
                        src.CurrentWorkflowId.HasValue
                            ? (DateTimeOffset.Now - src.CurrentWorkflow.DateCreate).Days
                            : 0))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.Request))
                .ForMember(dest => dest.TaskType, opt => opt.UseValue("request"))
                .ForMember(dest => dest.CanGenerateGosNumber, opt => opt.UseValue(false))
                .ForMember(dest => dest.Priority, opt => opt.ResolveUsing<TaskPriorityResolver>())
                .ForMember(dest => dest.RegNumber, opt => opt.MapFrom(src => src.RequestNum))
                .ForMember(dest => dest.IsComplete,
                    opt => opt.MapFrom(src =>
                        src.CurrentWorkflowId.HasValue &&
                        (src.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.UM_03_4_0 
                        //&& src.Workflows.Any(rw => rw.CurrentStage.Code == RouteStageCodes.UM_03_9) 
                        || (src.CurrentWorkflow.IsComplete ?? false))))
                .ForMember(dest => dest.CanDownload, opt => opt.MapFrom(src => src.MainAttachmentId.HasValue))
                .ForMember(dest => dest.IsIndustrial,
                    opt => opt.MapFrom(src =>
                        new[]
                        {
                            DicProtectionDocTypeCodes.RequestTypeInventionCode,
                            DicProtectionDocTypeCodes.RequestTypeUsefulModelCode,
                            DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode
                        }.Contains(src.ProtectionDocType.Code)))
                .ForMember(dest => dest.IpcCodes,
                    opt => opt.MapFrom(src =>
                        src.IPCRequests.Any()
                            ? string.Join(", ", src.IPCRequests.Select(i => i.Ipc.Code))
                            : string.Empty))
                .ForMember(dest => dest.CountIndependentItems, opt => opt.MapFrom(src => src.CountIndependentItems))
                .ForMember(dest => dest.SpeciesTrademarkCode,
                    opt => opt.MapFrom(src => src.SpeciesTradeMarkId.HasValue ? src.SpeciesTradeMark.Code : string.Empty))
                .ForMember(dest => dest.IsActiveProtectionDocument, opt => opt.UseValue(false));
        }

        internal class TaskPriorityResolver : IValueResolver<Domain.Entities.Request.Request, IntellectualPropertyDto, TaskPriority>
        {
            private readonly IExecutor _executor;
            private readonly List<string> _expirationStartsFromDateCreateOnStageCodes = new List<string>
            {
                RouteStageCodes.NMPT_03_2,
                RouteStageCodes.NMPT_02_2_0,
                RouteStageCodes.UM_02_2_7,
                RouteStageCodes.UM_02_2_2_2,
                RouteStageCodes.UM_03_2
            };
            private readonly List<string> _legalPersonsStageCodes = new List<string>
            {
                RouteStageCodes.NMPT_03_2_4,
                RouteStageCodes.NMPT_03_3_2
            };
            private readonly List<string> _cumulativeStageCodes = new List<string>
            {
                RouteStageCodes.UM_02_2_1
            };

            public TaskPriorityResolver(IExecutor executor)
            {
                _executor = executor;
            }
            public TaskPriority Resolve(Domain.Entities.Request.Request request, IntellectualPropertyDto dto, TaskPriority priority,
                ResolutionContext context)
            {
                var expirationType = request?.CurrentWorkflow?.CurrentStage?.ExpirationType;
                var expirationValue = request?.CurrentWorkflow?.CurrentStage?.ExpirationValue;
                DateTimeOffset? dateFrom = request?.CurrentWorkflow?.DateCreate;
                //Исключительные случаи
                if (_expirationStartsFromDateCreateOnStageCodes.Contains(request?.CurrentWorkflow?.CurrentStage?.Code))
                {
                    //Отсчет идет от даты подачи заявки
                    dateFrom = request?.RequestDate ?? null;
                }
                if (_legalPersonsStageCodes.Contains(request?.CurrentWorkflow?.CurrentStage?.Code))
                {
                    //Срок рассмотрения - месяц для юриков
                    var requestHasLegalDeclarants =
                        request?.RequestCustomers.Any(rc => rc.CustomerRole.Code == "1" && rc.Customer.Type.Code == "1");
                    if (requestHasLegalDeclarants != null && requestHasLegalDeclarants.Value)
                    {
                        expirationType = ExpirationType.CalendarMonth;
                        expirationValue = 1;
                    }
                }
                if (expirationType == ExpirationType.None || dateFrom == null || expirationValue == null)
                {
                    return TaskPriority.Normal;
                }
                var executionDate = _executor.GetHandler<CalculateExecutionDateHandler>()
                    .Process(h => h.Execute(dateFrom.Value, expirationType ?? ExpirationType.None, expirationValue ?? 0));


                if (_cumulativeStageCodes.Contains(request?.CurrentWorkflow?.CurrentStage?.Code))
                {
                    var dayOfMonth = request?.CurrentWorkflow?.DateCreate.Day;
                    if (dayOfMonth != null)
                    {
                        var dateCreate = request.CurrentWorkflow.DateCreate;
                        if (dayOfMonth < 10)
                        {
                            executionDate = new DateTimeOffset(dateCreate.Year, dateCreate.Month, 10, 0, 0, 0, dateCreate.Offset);
                        }
                        else if (dayOfMonth < 20)
                        {
                            executionDate = new DateTimeOffset(dateCreate.Year, dateCreate.Month, 20, 0, 0, 0,
                                dateCreate.Offset);
                        }
                        else
                        {
                            executionDate = new DateTimeOffset(dateCreate.AddMonths(1).Year,
                                dateCreate.AddMonths(1).Month, 1, 0, 0, 0, dateCreate.Offset);
                        }
                    }
                }

                if (RouteStageCodes.UM_03_8 == request?.CurrentWorkflow?.CurrentStage?.Code)
                {
                    //Расчет сроков публикации полезных моделей
                    executionDate = _executor.GetHandler<CalculateExecutionDateHandler>()
                        .Process(h => h.Execute(request.RequestDate ?? request.DateCreate, ExpirationType.CalendarMonth, 12));
                    var registerDifferenceInDays = (executionDate - DateTimeOffset.Now).Days;
                    if (registerDifferenceInDays < 4)
                    {
                        return TaskPriority.Red;
                    }
                    if (registerDifferenceInDays < 6)
                    {
                        return TaskPriority.Yellow;
                    }
                    return TaskPriority.Normal;
                }

                var differenceInDays = (DateTimeOffset.Now - executionDate).Days;
                if (differenceInDays < 1)
                {
                    return TaskPriority.Normal;
                }
                if (differenceInDays < 5)
                {
                    return TaskPriority.Yellow;
                }

                return TaskPriority.Red;
            }
        }
    }
}
