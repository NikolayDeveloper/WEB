using System;
using AutoMapper;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Model.Models.Journal;

namespace Iserv.Niis.Model.Mappings.Contract
{
    public class ContractDtoProfile : Profile
    {
        public ContractDtoProfile()
        {
            CreateMap<Domain.Entities.Contract.Contract, IntellectualPropertyDto>()
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
                    x.TypeId.HasValue
                        ? x.Type.NameRu
                        : string.Empty))
                .ForMember(dest => dest.ReviewDaysAll,
                    opt => opt.MapFrom(src => (DateTimeOffset.Now - src.DateCreate).Days))
                .ForMember(dest => dest.ReviewDaysStage,
                    opt => opt.MapFrom(src =>
                        src.CurrentWorkflowId.HasValue
                            ? (DateTimeOffset.Now - src.CurrentWorkflow.DateCreate).Days
                            : 0))
                .ForMember(dest => dest.RegNumber, opt => opt.MapFrom(src => src.ContractNum))
                .ForMember(dest => dest.OwnerType, opt => opt.UseValue(Owner.Type.Contract))
                .ForMember(dest => dest.TaskType, opt => opt.UseValue("contract"))
                .ForMember(dest => dest.CanGenerateGosNumber, opt => opt.UseValue(false))
                .ForMember(dest => dest.CanDownload, opt => opt.MapFrom(src => src.MainAttachmentId.HasValue))
                .ForMember(dest => dest.IsActiveProtectionDocument, opt => opt.UseValue(false));
        }
    }
}
